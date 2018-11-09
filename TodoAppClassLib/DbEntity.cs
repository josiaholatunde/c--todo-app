using System;
namespace TodoAppClassLib
{
    public abstract class DbEntity
    {
        protected DataAccess _dt;
        public DbEntity(DataAccess dt)
        {
            this._dt = dt;
            
        }
        
        internal  ObjectState State;

       
        public void Save()
        {
            if(State == ObjectState.New)
            {
                this.Insert();
            }
            else if(State == ObjectState.Changed)
            {
                this.Update();
             
            }
            else if(State == ObjectState.Removed)
            {
                this.Delete();
            }

        }

    internal void ChangeState()
    {
        if(State == ObjectState.Unchanged)
            State = ObjectState.Changed;
            
    }

        internal abstract void Insert();
        internal abstract void Update();
        internal abstract void Delete();
    }
}
