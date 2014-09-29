/*
 * This is the function that actually highlights a text string by
 * adding HTML tags before and after all occurrences of the search
 * term. You can pass your own tags if you'd like, or if the
 * highlightStartTag or highlightEndTag parameters are omitted or
 * are empty strings then the default <font> tags will be used.
 */
function doHighlightBody(bodyText, searchTerm, highlightStartTag, highlightEndTag) 
{

  
  // find all occurences of the search term in the given text,
  // and add some "highlight" tags to them (we're not using a
  // regular expression search, because we want to filter out
  // matches that occur within HTML tags and script blocks, so
  // we have to do a little extra validation)
  var newText = "";
  var i = -1;
  var lcSearchTerm = searchTerm.toLowerCase();
  var lcBodyText = bodyText.toLowerCase();
  
  while (bodyText.length > 0) {
    i = lcBodyText.indexOf(lcSearchTerm, i+1);
    if (i < 0) {
      newText += bodyText;
      bodyText = "";
    } else {

          // skip anything inside an HTML tag
          if (bodyText.lastIndexOf(">", i) >= bodyText.lastIndexOf("<", i)) 
          {
            // skip anything inside a <script> block
            if (lcBodyText.lastIndexOf("/script>", i) >= lcBodyText.lastIndexOf("<script", i)) 
            {
              newText += bodyText.substring(0, i) + highlightStartTag + bodyText.substr(i, searchTerm.length) + highlightEndTag;
              bodyText = bodyText.substr(i + searchTerm.length);
              lcBodyText = bodyText.toLowerCase();
              i = -1;
            }
          }

    }
  }

  
  return newText;
}


/*
 * This is the function that actually highlights a text string by
 * adding HTML tags before and after all occurrences of the search
 * term. You can pass your own tags if you'd like, or if the
 * highlightStartTag or highlightEndTag parameters are omitted or
 * are empty strings then the default <font> tags will be used. In addition, it 
 * only looks at the text of a specific elementId instead of all the body text.
 */
function doHighlightElement(bodyText, elementIdText, searchTerm, highlightStartTag, highlightEndTag) 
{

  
  // find all occurences of the search term in the given text,
  // and add some "highlight" tags to them (we're not using a
  // regular expression search, because we want to filter out
  // matches that occur within HTML tags and script blocks, so
  // we have to do a little extra validation)
  var oldElementIdText = elementIdText;
  var newText = "";
  var i = -1;
  var lcSearchTerm = searchTerm.toLowerCase();
  var lcBodyText = bodyText.toLowerCase();
  var lcElementIdText = elementIdText.toLowerCase();
  
  
  while (elementIdText.length > 0) {
    i = lcElementIdText.indexOf(lcSearchTerm, i+1);
    if (i < 0) {
      newText += elementIdText;
      elementIdText = "";
    } else {
          newText += elementIdText.substring(0, i) + highlightStartTag + elementIdText.substr(i, searchTerm.length) + highlightEndTag;
          elementIdText = elementIdText.substr(i + searchTerm.length);
          lcElementIdText = elementIdText.toLowerCase();
          i = -1;

    }
  }  
  
  var rexExp = new RegExp(oldElementIdText)  
  var newBodyText = bodyText.replace(rexExp,newText);
  
  return newBodyText;
}



/*
 * This is sort of a wrapper function to the doHighlight function.
 * It takes the searchText that you pass, optionally splits it into
 * separate words, and transforms the text on the current web page.
 * Only the "searchText" parameter is required; all other parameters
 * are optional and can be omitted.
 */
function highlightSearchTerms(searchText, elementId, treatAsPhrase, textColor, bgColor)
{
  // if the treatAsPhrase parameter is true, then we should search for 
  // the entire phrase that was entered; otherwise, we will split the
  // search string so that each word is searched for and highlighted
  // individually
  if (treatAsPhrase) {
    searchArray = [searchText];
  } else {
    searchArray = searchText.split(" ");
  }
 /* 
  if (!document.body || typeof(document.body.innerHTML) == "undefined") {
    if (warnOnFailure) {
      alert("Sorry, for some reason the text of this page is unavailable. Searching will not work.");
    }
    return false;
  }
   */
   
   var highlightStartTag = "";
   var highlightEndTag = "";   
   
  // we can optionally use our own highlight tag values
  if ((!textColor) || (!bgColor)) {
    highlightStartTag = "";
    highlightEndTag = "";
  } else {
    highlightStartTag = "<font style='color:" + textColor + "; background-color:" + bgColor + ";'>";
    highlightEndTag = "</font>";
  }   
   
  var elementIdText = document.getElementById(elementId).innerText;
  var bodyText = document.body.innerHTML;
  for (var i = 0; i < searchArray.length; i++) {
    //bodyText = doHighlightBody(bodyText, searchArray[i], highlightStartTag, highlightEndTag);
    bodyText = doHighlightElement(bodyText, elementIdText, searchArray[i], highlightStartTag, highlightEndTag);
  }
  
  
  document.body.innerHTML = bodyText;
  return true;
}


///*
// * This displays a dialog box that allows a user to enter their own
// * search terms to highlight on the page, and then passes the search
// * text or phrase to the highlightSearchTerms function. All parameters
// * are optional.
// */
//function searchPrompt(defaultText, treatAsPhrase, textColor, bgColor)
//{
//  // This function prompts the user for any words that should
//  // be highlighted on this web page
//  if (!defaultText) {
//    defaultText = "";
//  }
//  
//  // we can optionally use our own highlight tag values
//  if ((!textColor) || (!bgColor)) {
//    highlightStartTag = "";
//    highlightEndTag = "";
//  } else {
//    highlightStartTag = "<font style='color:" + textColor + "; background-color:" + bgColor + ";'>";
//    highlightEndTag = "</font>";
//  }
//  
//  
//  return highlightSearchTerms(searchText, treatAsPhrase, true, textColor, bgColor);
//}


///*
// * This function takes a referer/referrer string and parses it
// * to determine if it contains any search terms. If it does, the
// * search terms are passed to the highlightSearchTerms function
// * so they can be highlighted on the current page.
// */
//function highlightGoogleSearchTerms(referrer)
//{
//  // This function has only been very lightly tested against
//  // typical Google search URLs. If you wanted the Google search
//  // terms to be automatically highlighted on a page, you could
//  // call the function in the onload event of your <body> tag, 
//  // like this:
//  //   <body onload='highlightGoogleSearchTerms(document.referrer);'>
//  
//  //var referrer = document.referrer;
//  if (!referrer) {
//    return false;
//  }
//  
//  var queryPrefix = "q=";
//  var startPos = referrer.toLowerCase().indexOf(queryPrefix);
//  if ((startPos < 0) || (startPos + queryPrefix.length == referrer.length)) {
//    return false;
//  }
//  
//  var endPos = referrer.indexOf("&", startPos);
//  if (endPos < 0) {
//    endPos = referrer.length;
//  }
//  
//  var queryString = referrer.substring(startPos + queryPrefix.length, endPos);
//  // fix the space characters
//  queryString = queryString.replace(/%20/gi, " ");
//  queryString = queryString.replace(/\+/gi, " ");
//  // remove the quotes (if you're really creative, you could search for the
//  // terms within the quotes as phrases, and everything else as single terms)
//  queryString = queryString.replace(/%22/gi, "");
//  queryString = queryString.replace(/\"/gi, "");
//  
//  return highlightSearchTerms(queryString, false);
//}


///*
// * This function is just an easy way to test the highlightGoogleSearchTerms
// * function.
// */
//function testHighlightGoogleSearchTerms()
//{
//  var referrerString = "http://www.google.com/search?q=javascript%20highlight&start=0";
//  referrerString = prompt("Test the following referrer string:", referrerString);
//  return highlightGoogleSearchTerms(referrerString);
//}