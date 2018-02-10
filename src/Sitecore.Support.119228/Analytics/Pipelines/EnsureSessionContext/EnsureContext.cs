namespace Sitecore.Support.Analytics.Pipelines.EnsureSessionContext
{
  using Sitecore.Analytics.Data;
  using Sitecore.Analytics.Pipelines.InitializeTracker;
  using Sitecore.Diagnostics;
  using Sitecore.Sites;
  using Sitecore.Xdb.Configuration;
  using Sitecore.Analytics.Model;

  public class EnsureContext : InitializeTrackerProcessor
  {
    public override void Process(InitializeTrackerArgs args)
    {
      Assert.ArgumentNotNull(args, "args");
      Assert.IsNotNull(this.SessionContextManager, "HttpSessionContextManager is not configured");
      if (args.Session == null)
      {
        SiteContext site = Context.Site;
        if (XdbSettings.Enabled)
        {
          if ((site != null) && (site.DisplayMode != DisplayMode.Normal))
          {
            args.Session = this.SessionContextManager.EmptySession;
          }
          else
          {
            args.Session = this.SessionContextManager.GetSession();
          }
        }
        else
        {
          args.Session = this.SessionContextManager.GetSession();
          if (args.Session != null)
          {
            if (args.Session.Contact != null)
            {
              args.Session.Contact.ContactSaveMode = ContactSaveMode.NeverSave;
            }
            if (args.Session.Settings != null)
            {
              args.Session.Settings.IsTransient = true;
            }
          }
        }
      }
    }

    protected virtual SessionContextManagerBase SessionContextManager { get; set; }
  }
}
