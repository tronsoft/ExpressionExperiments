namespace ExpressionExperiments
{
    internal class Page
    {
        public Page()
        {
            
        }
        
        public Page(PageContext pageContext, ActionContext actionContext)
        {
            PageContext = pageContext;
            ActionContext = actionContext;
        }
        
        public PageContext PageContext { get; set; }
        public ActionContext ActionContext { get; set; }
    }
}