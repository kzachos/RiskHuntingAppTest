Rico.EventWrapper = Class.create();
Rico.EventWrapper.prototype = {
  initialize: function(handler, target){
    this.handler = handler;
    this.target = target;
    this.wrapper = this.wrapperCall.bindAsEventListener(this)
  },
  wrapperCall: function(event){
    this.handler(event, this.target)
  }
}
   
Rico.Ajax = {};

Rico.CachedAjax = {
  cacheTime: 600,
  cachedRequests: {}
}

Rico.CachedReplaceHTML = Class.create();
Rico.CachedReplaceHTML.prototype = {
  initialize:function(){
  }
}

Rico.Ajax.Request = Class.create();
Rico.Ajax.Request.prototype = {
  initialize:function(url, options){
    key = url;
    if (options.parameters)
      key += options.parameters    
    request = Rico.CachedAjax.cachedRequests[key]
    if (request)
      request.evalResponse();
    else
      Rico.CachedAjax.cachedRequests[key] = new Ajax.Request(url, options);
  }
}

Rico.selectionSet = function(set, options){
  new Rico.SelectionSet(set, options)
}

Rico.SelectionSet = Class.create(); 
Rico.SelectionSet.prototype = {
	initialize: function(selectionSet, options){
		this.options = options || {}
    if (typeof selectionSet == 'string')
      selectionSet = $$(selectionSet)
	  this.previouslySelected = [];
		this.selectionSet = selectionSet;
		this.selectedClassName = this.options.selectedClass || "selected";
		this.selectNode = this.options.selectNode || function(e){return e};
		this.onSelect = this.options.onSelect;
    this.onFirstSelect = this.options.onFirstSelect;
		this.clickHandler = this.click.bind(this);
		selectionSet.each(function(e) {Event.observe(e, "click", new Rico.EventWrapper(this.clickHandler,e).wrapper);}.bind(this))
    if (!this.options.noDefault)
		  this.selectIndex(this.options.selectedIndex || 0)
	},
	reset: function(){
	  this.previouslySelected = [];
	  this.notifySelected(this.selected);
	},
	select: function(element){
		if (this.selected == element)
			return;

		if (this.selected)
		  new Element.ClassNames(this.selectNode(this.selected)).remove(this.selectedClassName)

    this.notifySelected(element)

		this.selected = element;
		new Element.ClassNames(this.selectNode(this.selected)).add(this.selectedClassName)
	},
	notifySelected: function(element){
    var index = this.selectionSet.indexOf(element)
    if (this.onFirstSelect && !this.previouslySelected[index]){
      this.onFirstSelect(element, index)
      this.previouslySelected[index] = true;
    }
  	if (this.onSelect)
  	  this.onSelect(element, index)
	},
	selectIndex: function(index){
		this.select(this.selectionSet[index])
	},  
	click: function(event,target) {
		this.select(target);
	},
	add: function(item){
	//	this.selectionSet.push(item)
		Event.observe(item, "click", new Rico.EventWrapper(this.clickHandler,item).wrapper);
	},
	remove: function(item){
			//Todo: need to cleanup all events on item - need to keep track of eventwrappers
	},
	removeAll: function(){
		
	}
 }

Rico.HoverSet = Class.create();
Rico.HoverSet.prototype = {
    initialize: function(hoverSet, options){
      options = options || [];
      this.hoverSet = hoverSet;
      this.hoverClassName = options.hoverClass || "hover";
      this.hoverNodes = options.hoverNodes || function(e){return [e]};
  		this.listenerHover    = this._onHover.bind(this)
      this.listenerEndHover = this._onUnHover.bind(this)
      
  	  this.hoverSet.each((function(e) {Event.observe(e, "mousemove", new Rico.EventWrapper(this.listenerHover,e).wrapper);}).bind(this))
  	  this.hoverSet.each((function(e) {Event.observe(e, "mouseout", new Rico.EventWrapper(this.listenerEndHover,e).wrapper);}).bind(this))	
  	},
   	_onHover: function(event,target) {
   	  this.hover(target);
   	},	
   	_onUnHover: function(event,target) {
   	  this.unHover(target);
   	},
   	hover: function(target) {
   	  this.hoverNodes(target).each((function(t){Element.classNames(t).add(this.hoverClassName)}).bind(this));
   	},	
   	unHover: function(target) {
   	  this.hoverNodes(target).each((function(t){Element.classNames(t).remove(this.hoverClassName)}).bind(this));
   	},
		add: function(item){
  	  Event.observe(item, "mousemove", new Rico.EventWrapper(this.listenerHover,item).wrapper);
  	  Event.observe(item, "mouseout", new Rico.EventWrapper(this.listenerEndHover,item).wrapper);
		},
		remove: function(item){
			//Todo: need to cleanup all events on item - need to keep terack of eventwrappers
			//stopObserving
			//Event.stopObserving(e, "mousemove", new Rico.EventWrapper(this.listenerHover,e).wrapper);}).bind(this))
  	  //this.hoverSet.each((function(e) {Event.observe(e, "mouseout", new Rico.EventWrapper(this.listenerEndHover,e).wrapper);}).bind(this))
			//hoverSet
		},
		removeAll: function(item){
		}
}
 

Rico.Hover = {
  groups: {},
  clearCurrent: function(group) {
    var last_hover = Rico.Hover.groups[group];
    if(!last_hover) return  
    clearTimeout(last_hover[0])
    last_hover[1].end()
    Rico.Hover.groups[group] = null;
  }, 
  end: function(group) {
  	Rico.Hover.groups[group][1].end();
  },
  endWith: function(hover, group) {
  	var timer = setTimeout('Rico.Hover.end("'+ group + '")', hover.exitDelay)
    Rico.Hover.groups[group] = [timer, hover]
  }
}

Rico.HoverDisplay = Class.create();
Rico.HoverDisplay.prototype = {
  initialize: function(element, options) {
  	this.element = element;
  	this.options = options || {};
  	this.group = this.options.group;
  	this.exitDelay = this.options.delay || 1000;
  },
  begin: function() {
    Rico.Hover.clearCurrent(this.group)
		Element.show(this.element)
  },
  end: function(delay) {
    if(delay)
       	Rico.Hover.endWith(this, this.group);
    else 
		  Element.hide(this.element)		  
  }
}
