
function clearDebugPanel()
{
	var debugPanel = $("debugPanel");
	if (debugPanel) {
		debugPanel.innerHTML = "";
		debugPanel.style.visibility = "hidden";
		$("clearDebugPanelBtn").style.visibility = "hidden";
	}
}
		
function debug(msg, clearPanel)
{
	var debugPanel = $("debugPanel");
	if (!debugPanel) {
		document.body.appendChild(document.createElement("br"));

		// create scrollable outer div
		var outerPanel = document.createElement("div");
		outerPanel.style.height = "120px";
		outerPanel.style.overflow = "auto";
		document.body.appendChild(outerPanel);
		
		// create div for debug messages    
		debugPanel = document.createElement("div");
		debugPanel.id = "debugPanel";
		debugPanel.style.border = "1px solid black"; 
		debugPanel.style.background = "yellow";
		outerPanel.appendChild(debugPanel);
		
		// create "Clear" button 		
		var clearBtn = document.createElement("button");
		clearBtn.onclick = clearDebugPanel;
		clearBtn.id = "clearDebugPanelBtn";
		clearBtn.innerHTML = "Clear Debug Display";
		document.body.appendChild(clearBtn);
	}
	else {
		debugPanel.style.visibility = "visible";
		$("clearDebugPanelBtn").style.visibility = "visible";
	}
			
	if (clearPanel) {
	    debugPanel.innerHTML = msg;
	}
	else {
	    debugPanel.innerHTML += msg + "<br>";
	}
}

