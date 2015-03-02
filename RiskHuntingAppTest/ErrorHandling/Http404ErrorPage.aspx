<%@ Page Language="C#" %>

<script runat="server">
  protected HttpException ex = null;

  protected void Page_Load(object sender, EventArgs e)
  {
    // Log the exception and notify system operators
    ex = new HttpException("HTTP 404");
    ExceptionUtility.LogException(ex, "Caught in Http404ErrorPage");
    ExceptionUtility.NotifySystemOps(ex);
  }
</script>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" 
    "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html>
<head id="Head1" runat="server">
	<title>HTTP 404 Error Page</title>
	<link href="Theme/css/error.css" rel="stylesheet" type="text/css" />
</head>
<body>
  <form id="form1" runat="server">
  <div>
    <h1>Oops, something went wrong</h1>
    <div>
    We are currently having problems on the site.  An email has been sent to 
    the site owner to report the problem.
    <br>
    Return to the <a href='DescribeRisk.aspx'> Home Page</a>
    </div>
  </div>
  </form>
</body>
</html>


