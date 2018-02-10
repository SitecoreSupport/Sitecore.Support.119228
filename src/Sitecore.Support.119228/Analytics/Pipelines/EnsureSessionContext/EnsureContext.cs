namespace Sitecore.Support.Analytics.Pipelines.EnsureSessionContext
{
  using Sitecore.Analytics.Data;
  using Sitecore.Analytics.Pipelines.InitializeTracker;
  using Sitecore.Diagnostics;
  using Sitecore.Sites;

  public class EnsureContext : InitializeTrackerProcessor
  {
    public override void Process(InitializeTrackerArgs args)
    {
      Assert.ArgumentNotNull(args, "args");
      Assert.IsNotNull(this.SessionContextManager, "HttpSessionContextManager is not configured");
      if (args.Session == null)
      {
        SiteContext site = Context.Site;
        if ((site != null) && (site.DisplayMode != DisplayMode.Normal))
        {
          args.Session = this.SessionContextManager.EmptySession;
        }
        else
        {
          args.Session = this.SessionContextManager.GetSession();
        }
      }
    }

    protected virtual SessionContextManagerBase SessionContextManager { get; set; }
  }
}
