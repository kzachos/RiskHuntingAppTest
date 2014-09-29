//
// Based on version from: - Sylvain ZIMMER <sylvain _at_ jamendo.com>
//

Rico.Selector = Class.create();
Rico.Selector.prototype = {
  initialize: function(stack) {
    this.r=[]; 
    this.selectors=[]; 
    $A(stack).each(function(t){
      var s = {tag:"*",id:null,classNames:[]}; 
      var cursor = t.length-1;
      do {              
        var idIndex = t.lastIndexOf("#");
        var classIndex = t.lastIndexOf(".");             
        cursor = Math.max(idIndex,classIndex);
        if (cursor == -1)   
          s.tag = t.toUpperCase();
        else if (idIndex == -1 || classIndex == cursor)  
          s.classNames.push(t.substring(classIndex + 1));
        else if (!s.id)  
          s.id = t.substring(idIndex + 1);
        t = t.substring(0,cursor);
      } while (cursor>0);
      this.selectors.push(s);
    }.bind(this))
  },
  _findByIdOrTag: function(s, elt){
    if (!s.id)
      return $A(elt.getElementsByTagName(s.tag)); 
      
    var e = $(s.id);      
    if (e && (s.tag=="*" || e.tagName==s.tag) && Element.childOf(e, elt))
      return[e];
    else
      return []
  },
  _filterByClassName: function(classNames, results){
    if (classNames.length == 0) 
      return results;     
    return results.findAll(function(e){
      return this._checkClassNames(classNames, e)
    }.bind(this))
  },
  _checkClassNames: function(classNames, node){
    if (node.className.indexOf(" ") == -1) 
      return classNames.length == 1 && node.className == classNames[0]
    if (classNames.length == 1)
      return node.className.split(/\s+/).include(classNames[0]);
  
    var itemsClasses = node.className.split(/\s+/);  
    return s.classNames.all(function(c) {return itemsClasses.include(c);});
  },
  exploreAll: function(parent, selectorIndex) {
    var sel = this.selectors[selectorIndex]
    var results = this._findByIdOrTag(sel, parent);
    if (results.length == 0) 
      ;
      //results = getElementsByClassName(sel.classNames[0], parent)
    else
      results = this._filterByClassName(sel.classNames, results);
    if (selectorIndex >= this.selectors.length - 1) 
      return results;       
    return results.map((function(e) { return this.exploreAll(e, selectorIndex+1);}).bind(this)).flatten();   
  },
  exploreFirst: function(elt, selectors, selectorIndex) {
    var sel = this.selectors[selectorIndex]
    var results = this._findDirectChildren(sel, parent);
    if (results.length == 0)
      return parent.childNodes.find(function(n){this._find_directChildren(n,sel)}.bind(this)).first();
      
    return results.first();  
  },
  _findDirectChildren: function(parent, sel) {
     parent.childNodes.select(function(e){ 
      return !sel.tagName || (e && e.tagName && e.tagName == sel.tagName) &&
            sel.classNames.length ==0 || this._checkClassnames(sel.classNames, parent)})
  },
  findAll: function(root) {
    return this.exploreAll(root || document, 0);
  },
  findFirst: function(root){
    return this.findAll(root).first();
  },
  //supporting prototype interface
  findElements: function(root){
    return this.findAll(root);
  }
}


Rico.selector = function(str){
  return new Rico.Selector(str.split(/\s+/));
}


var $$old=$$;
var $$=function(str,args) {
  if (args || str.indexOf("[") >= 0) return $$old.apply(this,arguments);
  return new Rico.Selector(str.split(/\s+/)).findAll();
}
