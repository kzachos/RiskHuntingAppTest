// Base components - intended to be derived from
Rico.ContentTransitionBase = function() {}
Rico.ContentTransitionBase.prototype = {
	initialize: function(titles, contents, options) { 
	  this.titles = titles;
	  this.contents = contents;
		this.options = Object.extend({
			duration:200, 
			steps:8,
			rate:Rico.Effect.easeIn
	  }, options || {});
	  this.hoverSet = new Rico.HoverSet(titles, options);
		contents.each(function(p){ Element.hide(p)})
	  this.selectionSet = new Rico.SelectionSet(titles, Object.extend(this.options, {onSelect: this.select.bind(this)}));
		if (this.initContent) this.initContent();
	},
	reset: function(){
	  this.selectionSet.reset();
	},
	select: function(title) {
	  if ( this.selected == this.contentOf(title)) return
		var panel = this.contentOf(title); 
		if (this.transition){
			if (this.selected){
			  var effect = this.transition(panel)
			  if (effect) Rico.animate(effect, this.options)
      }
			else
				Element.show(panel);
		}else{
			if (this.selected)
				Element.hide(this.selected)
			Element.show(panel);
		}
		this.selected = panel;
	},
	add: function(title, content){
		this.titles.push(title);
		this.contents.push(content);
		this.hoverSet.add(title);
		this.selectionSet.add(title);	
		this.selectionSet.select(title);
	},
	remove: function(title){},
	removeAll: function(){
		this.hoverSet.removeAll();
		this.selectionSet.removeAll();
	},
	openByIndex: function(index){this.selectionSet.selectIndex(index)},
	contentOf: function(title){ return this.contents[this.titles.indexOf(title)]}
}

Rico.ContentTransition = Class.create();
Rico.ContentTransition.prototype = Object.extend(new Rico.ContentTransitionBase(),{});

Rico.SlidingPanel = Class.create();
Rico.SlidingPanel.prototype = {
	initialize: function(panel) {
		this.panel = panel;
		this.options = arguments[1] || {};
		this.closed = true;
		this.openEffect = this.options.openEffect;
		this.closeEffect = this.options.closeEffect;
		this.animator = new Rico.Effect.Animator();
		Element.makeClipping(this.panel)
	},
	toggle: function () {
		if(!this.showing)
			this.open();
		else 
			this.close();
		this.showing = !this.showing
	},	
	open: function () {
	  if (this.closed){
	    this.closed = false;
		  Element.show(this.panel);
  		this.options.disabler.disableNative();
    }
		this.animator.stop();
		this.animator.play(this.openEffect,
		 									{ onFinish:function(){ Element.undoClipping($(this.panel))}.bind(this),
												rate:Rico.Effect.easeIn});
	},
 	close: function () {
		Element.makeClipping(this.panel)
		this.animator.stop();
		this.animator.play(this.closeEffect,
	                     { onFinish:function(){ this.closed = true; Element.hide(this.panel); 	
																							this.options.disabler.enableNative()}.bind(this),	
												rate:Rico.Effect.easeOut});
	}
}


//-------------------------------------------
// Example components
//-------------------------------------------

function testFunction(){ 
		this.selected.style.height = this.options.panelHeight + "px";
	}
	
function testFunction2(p){ 
    if (!this.options.noAnimate)
		  return new Rico.AccordionEffect(this.selected, p, this.options.panelHeight);
    else{
      p.style.height = this.options.panelHeight + "px";
      if (this.selected) Element.hide(this.selected);
  		Element.show(p);
    }
	}	

Rico.Accordion = Class.create();
Rico.Accordion.prototype = Object.extend(new Rico.ContentTransitionBase(), {
  initContent: testFunction,
  transition: testFunction2
})
 

Rico.SlidingPanel.top = function(panel, innerPanel){
	var options = Object.extend({
		disabler: Rico.Controls.defaultDisabler
  }, arguments[2] || {});
	var height = options.height || Element.getDimensions(innerPanel)[1];
	var top = options.top || Position.positionedOffset(panel)[1];
	options.openEffect = new Rico.Effect.SizeFromTop(panel, innerPanel, top, height, {baseHeight:height});
	options.closeEffect = new Rico.Effect.SizeFromTop(panel, innerPanel, top, 1, {baseHeight:height});
	return new Rico.SlidingPanel(panel, options);
}

Rico.SlidingPanel.bottom = function(panel){
	var options = Object.extend({
		disabler: Rico.Controls.blankDisabler
  }, arguments[1] || {});
	var height = options.height || Element.getDimensions(panel).height;
	var top = Position.positionedOffset(panel)[1];
	options.openEffect = new Rico.Effect.SizeFromBottom(panel, top - height, height);
	options.closeEffect = new Rico.Effect.SizeFromBottom(panel, top, 1);
	return new Rico.SlidingPanel(panel, options); 
}

Rico.Event = {};
Rico.Event.modalWrapper = function(element, func){    
		Event.observe(element, "mousemove", Event.Stop);
		Event.observe(element, "mouseout", Event.Stop);
		Event.observe(element, "mousedown", func);
}
