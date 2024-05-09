﻿namespace Netflix_Server.IRepositorys
{
    public interface IRepository<T> where T : class
    {
        Task<List<T>> GetList();
        Task<T> GetById(int id);
        Task<T> GetByName(string name);

        Task Create(T item);
        void Update(T item);

        Task Delete(int id);
        Task Save();

        Task<bool> Exists(int id);


    }
}
