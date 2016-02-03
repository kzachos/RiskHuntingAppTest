using System;
using System.Collections;
using System.Collections.Generic;

namespace RiskHuntingAppTest
{
	public class GenericCreativityPrompts
	{
		public List<string> genericCPs; 

		public GenericCreativityPrompts (string filler, string type)
		{
			genericCPs = new List<string> ();

			if (type.Equals ("NP")) {
				this.genericCPs.Add ("how to make the " + filler + " move and adjust");
				this.genericCPs.Add ("doing the opposite of what is expected with the " + filler);
				this.genericCPs.Add ("if you can replace something mechanical in the " + filler + " with something that is sensory");
				this.genericCPs.Add ("if it is possible to change the density of the " + filler);
				this.genericCPs.Add ("if it is possible to change the temperature of the " + filler);
				this.genericCPs.Add ("making the " + filler + " more flexible");
				this.genericCPs.Add ("how to use liquid or air with the " + filler);
				this.genericCPs.Add ("if it is possible to regenerate the " + filler);
				this.genericCPs.Add ("how you provide a shell or cover for the " + filler);
				this.genericCPs.Add ("whether you can make a copy of the " + filler);
				this.genericCPs.Add ("how you might combine the " + filler + " with something else");
				this.genericCPs.Add ("if you could make do with more of the " + filler + ", or less of the " + filler);
				this.genericCPs.Add ("whether you can balance the " + filler + " with something else");
				this.genericCPs.Add ("how to make the " + filler + " work before it is needed");
				this.genericCPs.Add ("how to make the " + filler + " do lots of different things");
				this.genericCPs.Add ("how to introduce feedback into the " + filler);
				this.genericCPs.Add ("how to make the " + filler + " self-sustaining, so that it uses all of its waste");
				this.genericCPs.Add ("how to remove something from the " + filler);
				this.genericCPs.Add ("how to avoid stress in the " + filler + ", and/or the situation, before it happens");
				this.genericCPs.Add ("how to make parts or all of the " + filler + " move and adjust");
				this.genericCPs.Add ("how to make the " + filler + " vibrate");
				this.genericCPs.Add ("whether it is possible to make the " + filler + " change itself, release or absorb energy");
				this.genericCPs.Add ("whether it is possible to make the " + filler + " an irregular shape");
				this.genericCPs.Add ("trying to put the " + filler + " inside another thing, inside another thing");
				this.genericCPs.Add ("making the " + filler + " more spherical or circular");
				this.genericCPs.Add ("making all of the part of the " + filler + " of one substance");
				this.genericCPs.Add ("dividing the " + filler + " up");
				this.genericCPs.Add ("making the " + filler + " either transparent or a different colour");
				this.genericCPs.Add ("trying to make the " + filler + " expand or contract in response to its environment");
				this.genericCPs.Add ("either trying to put holes in the " + filler + " or to fill holes in the " + filler);
				this.genericCPs.Add ("making the " + filler + " cheap and disposable");
				this.genericCPs.Add ("making the " + filler + " pulse");
				this.genericCPs.Add ("putting the " + filler + " in a vacuum");
				this.genericCPs.Add ("deactivating the " + filler);
				this.genericCPs.Add ("evening out the environmental forces that affect the " + filler);
			} 
			else if (type.Equals ("VP")) {
				this.genericCPs.Add ("using materials that are composed of many things during " + filler);
				this.genericCPs.Add ("how to speed up " + filler);
				this.genericCPs.Add ("whether you can repeat " + filler);
				this.genericCPs.Add ("if you can use a messenger of some form in conjunction with " + filler);
				this.genericCPs.Add ("how to introduce feedback during " + filler);
				this.genericCPs.Add ("making " + filler + " self-sustaining, so that it recycles all of its waste");
				this.genericCPs.Add ("how to remove a step from " + filler);
				this.genericCPs.Add ("having an emergency plan in place for " + filler);
				this.genericCPs.Add ("how to distribute " + filler);
				this.genericCPs.Add ("how to add or use an extra dimension during " + filler);
				this.genericCPs.Add ("trying to enrich the environment in which " + filler + " take place");
				this.genericCPs.Add ("making something pulse during " + filler);
				this.genericCPs.Add ("evening out environmental forces that occur during " + filler);
			}
			else
				this.genericCPs.Add ("");

		}
	}
}

