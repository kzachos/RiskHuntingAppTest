/*--------------------------------------------------------
 * Copyright © 2009 – 2010* France Telecom
 * This software is distributed under the "Simplified BSD license",
 * the text of which is available at http://www.winktoolkit.org/licence.txt
 * or see the "license.txt" file for more details.
 *--------------------------------------------------------*/

doh.register("wink.math._basics",
	[
        // Test round
        function round(t)
        {
        	doh.is(3.14, wink.math.round(Math.PI, 2));
        }
    ]
);