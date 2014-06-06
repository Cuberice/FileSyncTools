using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Common;
using SyncMobile.Models;
using WebGrease.Css.Extensions;

namespace SyncMobile.Controllers
{
	public class HomeController : Controller
	{
		#region HttpGet

		public ActionResult RefreshData()
		{
			SyncUtils.UpdateFilesCollectionWeb();

			return RedirectToAction("Index");
		}

		//SyncGroup & SyncInformation - Paths are simple seperated by ListSeperator
		public ActionResult Synced()
		{
			ViewBag.Title = "Synchronized";
			ViewBag.Message = "Recently Synchronized groupd by Directory";

			List<SyncPath> col;
			try
			{
				col = SyncUtils.View_SyncedCollection();
			}
			catch
			{
				return RedirectToAction("Error");
			}

			SyncGroup sg = new SyncGroup {SyncInformations = GetInformationModelData(col).ToList()};
			sg.AllowEditWatch();
			return View(sg);
		}

		public ActionResult NotSynced()
		{
			ViewBag.Title = "Not Synchronized";
			ViewBag.Message = "All Files not yet Synchronized groupd by Directory";

			List<SyncPath> col;
			try
			{
				col = SyncUtils.View_GetNotSyncedCollection();
			}
			catch
			{
				return RedirectToAction("Error");
			}

			SyncGroup sg = new SyncGroup { SyncInformations = GetInformationModelData(col).ToList() };
			return View(sg);
		}

		public ActionResult ErrorFiles()
		{
			ViewBag.Title = "Errors";
			ViewBag.Message = "Files with Copy Error, Display Error or Not Downloaded";
			List<SyncPath> col = new List<SyncPath>();

			try
			{
				col = SyncUtils.View_GetErrorCollection();
			}
			catch
			{
				return RedirectToAction("Error");
			}

			SyncGroup sg = new SyncGroup { SyncInformations = GetInformationModelData(col).ToList() };
			sg.AllowEditWatch();
			return View(sg);
		}

		//PathGroup & PathInformation - Collapsable Container for each Path 
		public ActionResult Index()
		{
			ViewBag.Title = "What to Whatch Next";
			ViewBag.Message = "Next File to Whatch and Grouped by Directory for Recently Whatched Directories";
			List<SyncPath> col;
			try
			{
				col = SyncUtils.View_GetWatchCollection();
			}
			catch
			{
				return RedirectToAction("Error");
			}
			PathGroup sg = new PathGroup { PathInformations = GetPathModelData(col).ToList() };
			sg.SetAllowEdit(false, true);
			return View(sg);
		}

		public ActionResult NotWatched()
		{
			ViewBag.Title = "New Synchronized Files by Date";
			ViewBag.Message = "All Unwatched Files Ordered by Directory Synchronization Date and Grouped by Directory";
			List<SyncPath> col;
			try
			{
				col = SyncUtils.View_GetNotWatchedCollection();
			}
			catch
			{
				return RedirectToAction("Error");
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
				col = SyncUtils.View_GetAllCollection();
			}
			catch
			{
				return RedirectToAction("Error");
			}
			PathGroup sg = new PathGroup {PathInformations = GetPathModelData(col)};

			sg.SetAllowEdit(true, true);

			return View(sg);
		}
		
		public ActionResult Error()
		{
			ViewBag.ErrorMessage = "Failed "+ SyncUtils.GetDataConnectionString();
			return View();
		}

		//Partials
		public PartialViewResult ShowEditItem(string guid, int season, int episode, bool ismissing )
		{
			return PartialView("~/Views/Home/Partials/_EditItem.cshtml", new SyncInformation() { FileGUID = guid, Season = season, Episode = episode, IsMissing = ismissing });
		}	
		
		#endregion

		#region HttpPost

		[HttpPost]
		public ActionResult NotSynced(IList<SyncInformation> syncInformations)
		{
			SubmitData(syncInformations);
			return RedirectToAction("NotSynced");
		}

		[HttpPost]
		public ActionResult Index(IList<PathInformation> pathInformations)
		{
			SubmitData(pathInformations);
			return RedirectToAction("Index");
		}

		[HttpPost]
		public ActionResult NotWatched(IList<PathInformation> pathInformations)
		{
			SubmitData(pathInformations);
			return RedirectToAction("NotWatched");
		}

		[HttpPost]
		public ActionResult AllFiles(IList<PathInformation> pathInformations)
		{
			return RedirectToAction("AllFiles");
		}

		[HttpPost]
		public ActionResult ErrorFiles(IList<SyncInformation> syncInformations)
		{
			SubmitData(syncInformations);
			return RedirectToAction("ErrorFiles");
		}

		[HttpPost]
		public void WatchItem(string id, bool value)
		{
			if (true)
				SyncUtils.SubmitFilesWatch(id, value);
		}		
		
		[HttpPost]
		public void UpdateItem(string id, string season, string episode, bool issynced, bool ismissing)
		{
			
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
	}
}
