
//-------------------- rico.js
var Rico = {
  Version: 'current_build-54'
}

Rico.Effect = {};

Rico.URL = Class.create();
Rico.URL.prototype = {
    initialize : function(url){ 
      pair = url.split('?')
      this.basePath =  pair[0];
      this.params = this.extractParams(pair[1]);
    },
    extractParams: function (paramString) {
      if (!paramString) return {};
      return paramString.split('&').map(function(p){return p.split('amp;').last()});
    },
    getParamValue: function (name) {
      var matchName = name
      var param = $A(this.params).find(function(p){return matchName==p.split('=')[0]});
      return param ? param.split('=')[1] : null;
    },
    addParam: function(name, value){
      this.params.push(name +"="+ value)
    },
    setParam: function(name, value){
      var matchName = name
      this.params = $A(this.params).reject(function(p){return matchName==p.split('=')[0]});        
      this.addParam(name,value);
    },
    toS: function(){
      var paramString = this.params.join('&');
      return this.basePath + ((paramString != "") ? ("?" + paramString) : "");
    }    
}



//Rico.layout = {
//  makeYClipping: function(element) {
//    element = $(element);
//    if (element._overflowY) return;
//    element._overflow = element.style.overflow;
//    if ((Element.getStyle(element, 'yoverflow') || 'visible') != 'hidden')
//     ;
//      element.style.overflow-y = 'hidden';
//  },
//  undoYClipping: function(element) {
//    element = $(element);
//    if (element._overflowY) return;
//    element.style.overflow = element._overflowY;
//    element._overflowY = undefined;
//  }
//}


var RicoUtil = {

   getElementsComputedStyle: function ( htmlElement, cssProperty, mozillaEquivalentCSS) {
      if ( arguments.length == 2 )
         mozillaEquivalentCSS = cssProperty;

      var el = $(htmlElement);
      if ( el.currentStyle )
         return el.currentStyle[cssProperty];
      else
         return document.defaultView.getComputedStyle(el, null).getPropertyValue(mozillaEquivalentCSS);
   },
   createXmlDocument: function() {
      if (document.implementation && document.implementation.createDocument) {
         var doc = document.implementation.createDocument("", "", null);

         if (doc.readyState == null) {
            doc.readyState = 1;
            doc.addEventListener("load", function () {
               doc.readyState = 4;
               if (typeof doc.onreadystatechange == "function")
                  doc.onreadystatechange();
            }, false);
         }
         return doc;
      }
      if (window.ActiveXObject)
          return Try.these(
            function() { return new ActiveXObject('MSXML2.DomDocument')   },
            function() { return new ActiveXObject('Microsoft.DomDocument')},
            function() { return new ActiveXObject('MSXML.DomDocument')    },
            function() { return new ActiveXObject('MSXML3.DomDocument')   }
          ) || false;

      return null;
   },

   getContentAsString: function( parentNode ) {
      return parentNode.xml != undefined ? 
         this._getContentAsStringIE(parentNode) :
         this._getContentAsStringMozilla(parentNode);
   },

  _getContentAsStringIE: function(parentNode) {
     var contentStr = "";
     for ( var i = 0 ; i < parentNode.childNodes.length ; i++ ) {
         var n = parentNode.childNodes[i];
         if (n.nodeType == 4) {
             contentStr += n.nodeValue;
         }
         else {
           contentStr += n.xml;
       }
     }
     return contentStr;
  },

  _getContentAsStringMozilla: function(parentNode) {
     var xmlSerializer = new XMLSerializer();
     var contentStr = "";
     for ( var i = 0 ; i < parentNode.childNodes.length ; i++ ) {
          var n = parentNode.childNodes[i];
          if (n.nodeType == 4) { // CDATA node
              contentStr += n.nodeValue;
          }
          else {
            contentStr += xmlSerializer.serializeToString(n);
        }
     }
     return contentStr;
  },

   toViewportPosition: function(element) {
      return this._toAbsolute(element,true);
   },

   toDocumentPosition: function(element) {
      return this._toAbsolute(element,false);
   }
}


