using SPLConqueror_Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SPLConqueror_WebSite
{
    public partial class _Default : Page
    {
        private const string STATUS_NO_MODEL = "No model loaded.";
        private const string STATUS_NO_FUNCTION = "Model loaded, no function learned yet.";
        private const string STATUS_FUNCTION_LEARNED = "Model loaded, function learned.";

        public string learnedFunction = "";

        protected void Page_Load(object sender, EventArgs e)
        {
            if (GlobalState.varModel == null)
                status.Text = STATUS_NO_MODEL;
            else if (learnedFunction.Count() == 0)
                status.Text = STATUS_NO_FUNCTION;
            else
                status.Text = STATUS_FUNCTION_LEARNED;
        }
    }
}