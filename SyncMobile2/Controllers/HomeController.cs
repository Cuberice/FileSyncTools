﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Common;
using Models;
using SyncMobile.Models;
using SyncMobile2.MediaServiceReference;

namespace SyncMobile.Controllers
{
	public class HomeController : Controller
	{
		MediaSyncServiceClient MediaService = new MediaSyncServiceClient();

		#region HttpGet

		public ActionResult RefreshData()
		{
			SyncUtils.UpdateFilesCollectionWeb();

			return RedirectToAction("Index");
		}

		//SyncGroup & SyncInformation - Paths are simple seperated by ListSeperator
		public ActionResult Synced()
		{
			ViewBag.Title = "Last Synchronized Items";
			ViewBag.Message = "Recently Synchronized groupd by Directory";

			try
			{
				List<SyncPath> col = MediaService.Data_SyncedCollection();
				SyncGroup sg = new SyncGroup { SyncInformations = GetInformationModelData(col).ToList() };
				sg.AllowEditWatch();
				return View(sg);
			}
			catch (Exception c)
			{
				return RedirectToAction("Error", new { exceptionmessage = c });
			}
		}

		public ActionResult NotSynced()
		{
			ViewBag.Title = "Not Synchronized Yet";
			ViewBag.Message = "All Files not yet Synchronized groupd by Directory";

			List<SyncPath> col;
			try
			{
				col = MediaService.Data_GetNotSyncedCollection();
			}
			catch (Exception c)
			{
				return RedirectToAction("Error", new { exceptionmessage = c });
			}

			SyncGroup sg = new SyncGroup { SyncInformations = GetInformationModelData(col).ToList() };
			return View(sg);
		}

		public ActionResult ErrorFiles()
		{
			ViewBag.Title = "Errors or Missing Items";
			ViewBag.Message = "Files with Copy Error, Display Error or Not Downloaded";
			List<SyncPath> col = new List<SyncPath>();

			try
			{
				col = MediaService.Data_GetErrorCollection();
			}
			catch (Exception c)
			{
				return RedirectToAction("Error", new { exceptionmessage = c });
			}

			SyncGroup sg = new SyncGroup { SyncInformations = GetInformationModelData(col).ToList() };
			sg.AllowEditWatch();
			return View(sg);
		}

		//PathGroup & PathInformation - Collapsable Container for each Path 
		public ActionResult Index()
		{
			ViewBag.Title = "What to Whatch Next";
			ViewBag.Message = "Next File to Watch and Grouped by Directory for Recently Whatched Directories";
			List<SyncPath> col;
			try
			{
				col = MediaService.Data_GetWatchCollection();
			}
			catch(Exception c)
			{
				return RedirectToAction("Error", new { exceptionmessage = c});
			}
			PathGroup sg = new PathGroup { PathInformations = GetPathModelData(col).ToList() };
			sg.SetAllowEdit(false, true);
			return View(sg);
		}

		public ActionResult NotWatched()
		{
			ViewBag.Title = "Not Watched Yet";
			ViewBag.Message = "All Unwatched Files Ordered by Directory Synchronization Date and Grouped by Directory";
			List<SyncPath> col;
			try
			{
				col = MediaService.Data_GetNotWatchedCollection();
			}
			catch (Exception c)
			{
				return RedirectToAction("Error", new { exceptionmessage = c });
			}
			PathGroup sg = new PathGroup { PathInformations = GetPathModelData(col).ToList() };
			sg.SetAllowEdit(false, true);
			return View(sg);
		}

		public ActionResult AllFiles()
		{
			ViewBag.Title = "All Files";
			ViewBag.Message = "All Files Ordered by Directory Name - Identify Not Synced, Not Watched, Error and Watched";
			List<SyncPath> col;
			try
			{
				col = MediaService.Data_GetAllCollection();
			}
			catch (Exception c)
			{
				return RedirectToAction("Error", new { exceptionmessage = c });
			}
			PathGroup sg = new PathGroup {PathInformations = GetPathModelData(col)};

			sg.SetAllowEdit(true, true);

			return View(sg);
		}

		public ActionResult Error(string exceptionmessage)
		{
			ViewBag.ConnectionDetails = "Failed On:"+ SyncUtils.GetDataConnectionString();
			ViewBag.ExceptionMessage = exceptionmessage;
			return View();
		}

		//Partials
		public PartialViewResult ShowEditItem(string guid, int season, int episode, bool ismissing, bool issynced )
		{
			return PartialView("~/Views/Home/Partials/_EditItem.cshtml", 
				new SyncInformation()
				{
					FileGUID = guid, 
					Season = season, 
					Episode = episode, 
					IsSynced = issynced, 
					IsMissing = ismissing
				});
		}	
		
		#endregion

		#region HttpPost

		[HttpPost]
		public void WatchItem(string id, bool value)
		{
			SyncUtils.SubmitFilesWatch(id, value);
		}		
		
		[HttpPost]
		public void UpdateItem(string id, string season, string episode, bool issynced, bool ismissing)
		{
			SyncFile f = new SyncFile(id, issynced, ismissing, Convert.ToInt32(season), Convert.ToInt32(episode));
			SyncUtils.SubmitFileUpdate(f);
		}		
		
		[HttpPost]
		public void DeleteItem(string id)
		{
			Guid guid = Guid.Parse(id);
			SyncUtils.SubmitFileDelete(guid);
		}

		#endregion

		#region SyncPath SyncInformation Helpers

		private IEnumerable<SyncInformation> GetInformationModelData(IEnumerable<SyncPath> spList)
		{
			List<SyncInformation> list = new List<SyncInformation>();

			foreach (SyncPath sp in spList)
			{
				bool firstpath = true;
				foreach (SyncFile sf in sp.Files)
				{
					list.Add(new SyncInformation
					{
						FileGUID = sf.ID.ToString(),
						IsPath = firstpath,
						PathName = sp.Name,
						PathFileCount = sp.Files.Count,
						FileName = sf.File.Name,
						FileDate = sf.FileDate.HasValue ? sf.FileDate.Value.ToShortDateString() : sf.FileDate.ToString(),
						IsSynced = sf.IsSynced,
						IsWatched = sf.IsWatched,
						Season = sf.Season.HasValue ? sf.Season.Value : 0,
						Episode = sf.Episode.HasValue ? sf.Episode.Value : 0,
						Error = sf.Error,
						IsMissing = sf.IsMissing
					});

					firstpath = false;
				}
			}

			return list;
		}
		private List<SyncFile> GetSyncPathData(IEnumerable<SyncInformation> syncList)
		{
			List<SyncFile> sfList = new List<SyncFile>();
			foreach (SyncInformation si in syncList)
			{
				sfList.Add(new SyncFile(si.FileGUID, si.IsWatched, si.IsSynced, si.Season, si.Episode, DateTime.Now)
				{
					AllowIsSyncEdit = si.AllowIsSyncEdit, 
					AllowIsWatchedEdit = si.AllowIsWatchedEdit, 
					Error = si.Error,
					IsMissing = si.IsMissing
				});
			}

			return sfList;
		}
		protected void SubmitData(IList<SyncInformation> syncInformations)
		{
			if (syncInformations == null || !syncInformations.Any())
				return; 

			List<SyncFile> col = GetSyncPathData(syncInformations);
			SyncUtils.SubmitFilesWeb(col);
		}

		private List<SyncFile> GetSyncPathData(IEnumerable<PathInformation> syncList)
		{
			return (from pi in syncList
				from fi in pi.FileInformations
				select new SyncFile(fi.FileGUID, fi.IsWatched, fi.IsSynced, fi.Season, fi.Episode, DateTime.Now)
				{
					AllowIsSyncEdit = fi.AllowIsSyncEdit, AllowIsWatchedEdit = fi.AllowIsWatchedEdit, Error = fi.Error, IsMissing = fi.IsMissing
				}).ToList();
		}
		protected void SubmitData(IList<PathInformation> pathInformations)
		{
			if (pathInformations == null || !pathInformations.Any())
				return;

			List<SyncFile> col = GetSyncPathData(pathInformations);
			SyncUtils.SubmitFilesWeb(col);
		}
		private List<PathInformation> GetPathModelData(IEnumerable<SyncPath> spList)
		{
			return spList.Select(sp =>
								new PathInformation
								{
									PathName = sp.Name,
									PathFileCount = sp.Files.Count,
									FileInformations = sp.Files.Select(sf =>
											new FileInformation
											{
												FileGUID = sf.ID.ToString(),
												FileName = sf.File.Name,
												FileDate = sf.FileDate.HasValue ? sf.FileDate.Value.ToShortDateString() : sf.FileDate.ToString(),
												IsSynced = sf.IsSynced,
												IsWatched = sf.IsWatched,
												Error = sf.Error,
												Season = sf.Season.HasValue ? sf.Season.Value : 0,
												Episode = sf.Episode.HasValue ? sf.Episode.Value : 0,
												IsMissing = sf.IsMissing
											}).ToList()
								}).ToList();
		}

		private List<SyncFile> GetSyncPathDataFromInformation(IEnumerable<SyncInformation> syncList)
		{
			List<SyncFile> sfList = new List<SyncFile>();
			foreach (SyncInformation si in syncList)
			{
				sfList.Add(new SyncFile(si.FileGUID, si.IsWatched, si.IsSynced, si.Season, si.Episode, DateTime.Now)
				{
					AllowIsSyncEdit = si.AllowIsSyncEdit,
					AllowIsWatchedEdit = si.AllowIsWatchedEdit,
					Error = si.Error,
					IsMissing = si.IsMissing
				});
			}

			return sfList;
		}

		#endregion

		#region Test
		public ActionResult Test()
		{
			SyncInformation s = new SyncInformation {FileGUID = "TestGuid", IsPath = true, PathName = "Test"};

			return View(s);
		}

		public PartialViewResult TestAction(string fileguid)
		{
			SyncInformation s = new SyncInformation {FileGUID = "PartialGuid", IsPath = true, PathName = "Partial"};
			return PartialView("_TestPartial", s);
		}

		#endregion

		#region Example Code

//		[HttpPost]
//		public ActionResult NotSynced(IList<SyncInformation> syncInformations)
//		{
//			SubmitData(syncInformations);
//			return RedirectToAction("NotSynced");
//		}
//
//		[HttpPost]
//		public ActionResult Index(IList<PathInformation> pathInformations)
//		{
//			SubmitData(pathInformations);
//			return RedirectToAction("Index");
//		}

		#endregion
	}
}
