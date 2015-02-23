<%@ Page Language="C#" Inherits="RiskHuntingAppTest.DefaultRedirectErrorPage" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" 
    "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html>
<head id="Head1" runat="server">
	<title>DefaultRedirect Error Page</title>
	<link href="Theme/css/error.css" rel="stylesheet" type="text/css" />
</head>
<body>
  <form id="form1" runat="server">
  <div>
    <h1>Oops, something went wrong</h1>
    <div>
    We are currently having problems on the site.  An email has been sent to 
    the site owner to report the problem.
    <br><br>
    Return to the <a href='Default.aspx'> Home Page</a>
    </div>
  </div>
  </form>
</body>
</html>