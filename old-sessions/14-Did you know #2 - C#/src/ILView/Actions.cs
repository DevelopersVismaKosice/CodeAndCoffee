using System;
using System.Collections.Generic;
using System.Linq;

namespace DidYouKnowCSharp2.ILView
{
    public class Actions
    {
        public static List<IAction> GetActions()
        {
            List<IAction> actions = new List<IAction>();
            var abstracts = Enumerable.Range(1, 1000).Select(x => new ActionConfigAbstract() 
                { Name = x.ToString() }).ToList(); 
            
            foreach (ActionConfigAbstract ac in abstracts)
            {
                    actions.RemoveAll(delegate (IAction a) { return a.Name == ac.Name; });
                    actions.RemoveAll(a => new Actions().Tasker(a, ac));
                    actions.RemoveAll(a => a.Name == ac.Name);
                    actions.RemoveAll((IAction a) =>
                    {
                        int i = 0;
                        return a.Name == ac.Name;
                    });
                
            }
            return actions;
        }
        private bool Tasker(IAction a, ActionConfigAbstract ac)
        {
        return a.Name == ac.Name;
        }   
    }

    public interface IAction
    {
        string Name {get;set;}
    }
                
    public class ActionConfigAbstract : IAction
    {
        public string Name {get;set;}
    }
}