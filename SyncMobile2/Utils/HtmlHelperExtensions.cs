using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web.Mvc;
using System.Web.Mvc.Html;
using Common;
using Microsoft.Ajax.Utilities;
using SyncMobile.Models;


namespace SyncMobile.Utils
{
	public static class HtmlHelperExtensions
	{
		#region Original

		public static MvcHtmlString FileListViewItem<TModel>(this HtmlHelper<TModel> helper, List<SyncFile> list)
		{
			string strHtml = string.Empty;
			const string strChecked = "checked = 'checked'";
			const string strDisabled = "disabled=''";

			const string f0 = "<li><div class='ui-grid-b'><div class='ui-block-a'>{0}</div> {1}{2} </div></li>";
			const string flbl = "<h3>{0}</h3><p>Season: {1} - Ep: {2}</p>"; //0
			const string f1 = "<div class='ui-block-b'> <br><br><p>Date: {0} </p> </div>"; //1
			const string f2 = "<div class='ui-block-c'><div class='ui-grid-a' align='right'>{0}{1}</div></div>"; //2
			const string fcbx1 = "<div class='ui-block-a'><label><input name='checkbox-h-2a' id='checkbox-h-2a' type='checkbox' data-theme='b' {0} {1} >Synced</label></div>";
			const string fcbx2 = "<div class='ui-block-b'><label><input name='checkbox-h-2b' id='checkbox-h-2b' type='checkbox' data-theme='b' {0} {1} >Watched</label></div>";

			foreach (SyncFile f in list)
			{
				string lbl = string.Format(flbl, f.File.Name, "1", "01");
				string cbx1 = string.Format(fcbx1, f.IsSynced ? strChecked : string.Empty, strDisabled);
				string cbx2 = string.Format(fcbx2, f.IsWatched ? strChecked : string.Empty, f.IsSynced ? string.Empty : strDisabled);
					//Wathced
				string sf1 = string.Format(f1, f.FileDate.HasValue ? f.FileDate.Value.ToShortDateString() : f.FileDate.ToString());

				string sf0 = string.Format(f0, lbl, sf1, string.Format(f2, cbx1, cbx2));
				strHtml += sf0;
			}

			return MvcHtmlString.Create(strHtml);
		}

		public static MvcHtmlString PathListViewItem<TModel>(this HtmlHelper<TModel> helper, SyncPath p)
		{
			return
				new MvcHtmlString(string.Format("<li class='center' data-role='list-divider'>{0}<span class='ui-li-count'>{1}</span></li>", p.Name, p.Files.Count));
		}

		public static MvcHtmlString GroupListViewItem<TModel>(this HtmlHelper<TModel> helper, List<SyncPath> list)
		{
			try
			{
				string strHtml = string.Empty;
				foreach (SyncPath sp in list)
				{
					if (!sp.Files.Any())
						continue;

					strHtml += helper.PathListViewItem(sp) + helper.FileListViewItem(sp.Files).ToString();
				}

				if (string.IsNullOrEmpty(strHtml))
					strHtml = string.Format("<li>No Files Available... </li>");

				return new MvcHtmlString(strHtml);
			}
			catch
			{
				return new MvcHtmlString("<li> Exception in GroupListViewItem.. </li>");
			}
		}

		#endregion
		
		#region SyncInformtion Model

		public static MvcHtmlString FileListViewItem<TModel, TProperty>(this HtmlHelper<TModel> helper, Expression<Func<TModel, TProperty>> f, int id)
		{

			string strHtml = string.Empty;
			const string strChecked = "checked = 'checked'";
			const string strDisabled = "disabled=''";

			const string f0 = "<li><div class='ui-grid-b'><div class='ui-block-a'>{0}</div> {1}{2} </div></li>";
			const string flbl = "<h3>{0}</h3><p>Season: {1} - Ep: {2}</p>"; //0
			const string f1 = "<div class='ui-block-b'> <br><br><p>Date: {0} </p> </div>"; //1
			const string f2 = "<div class='ui-block-c'><div class='ui-grid-a' align='right'>{0}{1}</div></div>"; //2
			const string fcbx1 = "<div class='ui-block-a'><label><input name='[{2}].IsSynced' id='checkbox-h-2a' type='checkbox' data-theme='b' {0} {1} >Synced</label></div>";
			const string fcbx2 = "<div class='ui-block-b'><label><input name='[{2}].IsWatched' id='checkbox-h-2b' type='checkbox' data-theme='b' {0} {1} >Watched</label></div>";

			Func<TModel, TProperty> deleg = f.Compile();
			TProperty result = deleg(helper.ViewData.Model);
			SyncInformation sim = result as SyncInformation;

			string lbl = string.Format(flbl, sim.FileName, sim.Season, sim.Episode);
			string cbx1 = string.Format(fcbx1, sim.IsSynced ? strChecked : string.Empty, strDisabled, id);
			string cbx2 = string.Format(fcbx2, sim.IsWatched ? strChecked : string.Empty, sim.IsSynced ? string.Empty : strDisabled, id);
			string sf1 = string.Format(f1, sim.FileDate);

			string sf0 = string.Format(f0, lbl, sf1, string.Format(f2, cbx1, cbx2));
			strHtml = sf0;

			return MvcHtmlString.Create(strHtml);
		}

		#endregion
		
		#region Controls

		public static MvcHtmlString HeaderFor<TModel, TProperty>(this HtmlHelper<TModel> helper, Expression<Func<TModel, TProperty>> f, int headervalue)
		{
			Func<TModel, TProperty> deleg = f.Compile();
			TProperty result = deleg(helper.ViewData.Model);

			return new MvcHtmlString(string.Format("<h{1}>{0}</h{1}>", result, headervalue));
		}
		public static MvcHtmlString HeaderBubbleFor<TModel, TProperty>(this HtmlHelper<TModel> helper, Expression<Func<TModel, TProperty>> f, int headervalue, Func<TModel, string> fbubblevalue)
		{
			Func<TModel, TProperty> deleg = f.Compile();
			TProperty result = deleg(helper.ViewData.Model);

			return new MvcHtmlString(string.Format("<h{1}>{2}{0}</h{1}>", result, headervalue, string.Format("<div class=\"ui-btn-up-a ui-btn-corner-all custom-count-pos\">{0}</div>", fbubblevalue(helper.ViewData.Model))));
		}		
		
		public static MvcHtmlString ParagraphFor<TModel>(this HtmlHelper<TModel> helper, Func<TModel, string> f)
		{
			string result = f(helper.ViewData.Model);
			return new MvcHtmlString(string.Format("<p>{0}</p>", result));
		}

		public static MvcHtmlString PathListViewFor<TModel>(this HtmlHelper<TModel> helper, Func<TModel, string> fname, Func<TModel, int> ffilecount)
		{
			return new MvcHtmlString(string.Format("<li class='center' data-role='list-divider'>{0}<span class='ui-li-count'>{1}</span></li>", fname(helper.ViewData.Model), ffilecount(helper.ViewData.Model)));
		}

		public static MvcHtmlString CheckBoxLableFor<TModel>(this HtmlHelper<TModel> helper, Expression<Func<TModel, bool>> f, string theme, bool enabled, object htmlattributes)
		{
			return new MvcHtmlString(helper.LabelFor(f) + helper.CheckBoxFor(f, "b", true, htmlattributes).ToString());
		}
		public static MvcHtmlString CheckBoxFor<TModel>(this HtmlHelper<TModel> helper, Expression<Func<TModel, bool>> f, string theme, Func<TModel, bool> ef, object htmlattributes)
		{
			return helper.CheckBoxFor(f, theme, ef(helper.ViewData.Model), htmlattributes);
		}
		public static MvcHtmlString CheckBoxFor<TModel>(this HtmlHelper<TModel> helper, Expression<Func<TModel, bool>> f, string theme, bool enabled, object htmlattributes)
		{
			string str = string.Format(helper.CheckBoxFor(f, htmlattributes).ToString().Replace("type=\"checkbox\"", "type=\"checkbox\" data-theme='{0}' {1}"), 
				string.IsNullOrEmpty(theme) ? "a" : theme,
				enabled ? string.Empty : "disabled=''");

			return new MvcHtmlString(str);
		}

		public static MvcHtmlString CreateActionLink<TModel>(this HtmlHelper<TModel> helper, Options options, string attributes)
		{
			string str = helper.ActionLink(options.Text, options.Action, options.Controller).ToString().Replace("href=", attributes + "href=");
			return new MvcHtmlString(str);
		}

		public static MvcHtmlString Anchor<TModel>(this HtmlHelper<TModel> helper, Options options, bool mini)
		{
			string anchor = string.Format("<a href=\"/{0}/{1}\">Text Here</a>", options.Controller, options.Action);

			return new MvcHtmlString(anchor);
		}		
		public static MvcHtmlString IconAnchor<TModel>(this HtmlHelper<TModel> helper, Options options)
		{
			string anchor = string.Format("<a href=\"/{0}/{1}\" {2}></a>", options.Controller, options.Action,
				string.Format("{0}", options.Icon));

			return new MvcHtmlString(anchor);

			/*new { @class = "ui-btn-active" }*/
		}
		public static MvcHtmlString IconButton<TModel>(this HtmlHelper<TModel> helper, Options options,  bool mini)
		{
			return CreateActionLink(helper, options, string.Format("{0} {1} {2} ", JQuery.DataInline, mini ? JQuery.DataMini : "",  options.Icon ));
		}		
		public static MvcHtmlString InlineButton<TModel>(this HtmlHelper<TModel> helper, Options options)
		{
			return CreateActionLink(helper, options, 
				string.Format("{0} {1} {2} {3}", 
				JQuery.DataInline, 
				JQuery.DataMini, 
				options.Icon,
				options.IconOnly ? JQuery.DataIconPositionNoText : ""));
		}

		public static MvcHtmlString NumberBoxFor<TModel>(this HtmlHelper<TModel> helper, Func<TModel, string> f)
		{
			string result = f(helper.ViewData.Model);
			string input = string.Format("<input type=\"number\">{0}</input>", result);

			return new MvcHtmlString(input);
		}	
		
		public static class JQuery
		{
			public const string DataInline = "data-inline=\"true\"";
			public const string DataMini = "data-mini=\"true\"";
			public const string DataIconPositionNoText = "data-iconpos=\"notext\"";
			public const string DataIconLeft = "data-iconpos=\"left\"";
		}
		#endregion

		#region Helpers

		public static string GetDisplayName<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> f)
		{
			var metaData = ModelMetadata.FromLambdaExpression<TModel, TProperty>(f, htmlHelper.ViewData);
			return metaData.DisplayName ?? (metaData.PropertyName ?? ExpressionHelper.GetExpressionText(f));
		}

		#endregion

		#region Not Used
		public static MvcHtmlString MyCheckBoxForNameIndex<TModel, TProperty>(this HtmlHelper<TModel> helper, Expression<Func<TModel, TProperty>> f, string theme, int id)
		{
			Func<TModel, TProperty> deleg = f.Compile();
			TProperty result = deleg(helper.ViewData.Model);
			bool chk = Convert.ToBoolean(result.ToString());
			string displayname = GetDisplayName(helper, f);

			string str = string.Format("<label><input name='[{3}].IsPath' id='[{3}].IsPath' type='checkbox' data-theme='{1}' {0} >{2}</label>",
					chk ? "checked = 'checked'" : string.Empty,
					string.IsNullOrEmpty(theme) ? "a" : theme,
					displayname,
					id);

			return new MvcHtmlString(str);
		}

		public static MvcHtmlString MyCheckBoxForLi<TModel, TProperty>(this HtmlHelper<TModel> helper, Expression<Func<TModel, TProperty>> f, string theme, int id)
		{
			Func<TModel, TProperty> deleg = f.Compile();
			TProperty result = deleg(helper.ViewData.Model);
			bool chk = Convert.ToBoolean(result.ToString());
			string displayname = GetDisplayName(helper, f);

			string str =
				string.Format("<li class='center' data-role='list-divider'>><label><input name='SF{3}' id='SF{3}' type='checkbox' data-theme='{1}' {0} >{2}</label></li>",
					chk ? "checked = 'checked'" : string.Empty,
					string.IsNullOrEmpty(theme) ? "a" : theme,
					displayname,
					id
					);

			return new MvcHtmlString(str);
		}	
		#endregion
	}

	public class Options
	{
		public Options(string controller, string action)
		{
			Controller = controller;
			Action = action;
		}

		public string Controller;
		public string Action;
		public string Text;
		
		public string Icon;
		public bool IconOnly;
	}
	public static class Icons
	{
		public const string Refresh = "data-icon=\"refresh\"";
		public const string CheckMark = "data-icon=\"check\"";
		public const string Star = "data-icon=\"star\"";
		public const string Alert = "data-icon=\"alert\"";
		public const string Check = "data-icon=\"check\"";
		public const string Delete = "data-icon=\"delete\"";
		public const string Plus = "data-icon=\"plus\"";
		public const string ArrowDown = "data-icon=\"arrow-d\"";
		public const string Minus = "data-icon=\"minus\"";
		public const string Info = "data-icon=\"info\"";
		public const string Home = "data-icon=\"home\"";
		public const string Grid = "data-icon=\"grid\"";
	}
}