using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Eryan
{
    public class BaseSubject : Subject
    {
        List<Observer> observers = new List<Observer>();
        Event e;
        
        public void registerObserver(Observer o)
        {
            foreach (Observer observer in observers)
            {
                if (observer.Equals(o))
                    return;
            }
            observers.Add(o);
        }

        public void removeObserver(Observer o)
        {
            observers.Remove(o);
        }

        public void notifyObservers()
        {
            foreach (Observer observer in observers)
            {
                observer.update(e);
            }
        }
    }
}
