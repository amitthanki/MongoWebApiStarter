﻿namespace MongoWebApiStarter.Biz.Base
{
    /// <summary>
    /// Base class for models
    /// </summary>
    /// <typeparam name="TRepo">The type of repo for the model</typeparam>
    public abstract class ModelBase<TRepo> where TRepo : new()
    {
        protected TRepo Repo { get; set; } = new TRepo();
        public abstract void Save();
        public abstract void Load();
    }
}
