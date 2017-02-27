using MsDev.Taskschd.Core.EnumTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace MsDev.Taskschd.Core.Repositories
{
    /// <summary>
    /// 定义 数据代理 基本接口
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    public interface IRepository<TEntity> where TEntity : class, new()
    {
        #region query

        TEntity Find(int id);

        TEntity Find(Expression<Func<TEntity, bool>> predicate);

        Task<TEntity> FindAsync(int id);

        Task<TEntity> FindAsync(Expression<Func<TEntity, bool>> predicate);

        /// <summary>
        /// 获取总数
        /// </summary>
        /// <returns></returns>
        int Count();

        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="skip">0</param>
        /// <param name="take">12</param>
        /// <returns></returns>
        (int Count, IEnumerable<TEntity> Entities) PageList<TKey>(Expression<Func<TEntity, bool>> predicate, OrderTypes orderType = OrderTypes.不排序, Expression<Func<TEntity, TKey>> keySelector = null, int page = 0, int size = 10);

        Task<(int Count, IEnumerable<TEntity> Entities)> PageListAsync<TKey>(Expression<Func<TEntity, bool>> predicate, OrderTypes orderType = OrderTypes.不排序, Expression<Func<TEntity, TKey>> keySelector = null, int page = 0, int size = 10);

        #endregion query

        #region add

        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        TEntity Add(TEntity entity, bool save = true);

        Task<TEntity> AddAsync(TEntity entity, bool save = true);

        /// <summary>
        /// 批量添加
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        int AddRange(IEnumerable<TEntity> list, bool save = true);

        Task<int> AddRangeAsync(IEnumerable<TEntity> list, bool save = true);

        #endregion add

        #region update

        bool Update(TEntity entity, bool save = true);

        Task<bool> UpdateAsync(TEntity entity, bool save = true);

        #endregion update

        #region save

        bool Save();

        Task<bool> SaveAsync();

        #endregion save

        #region delete

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        int Delete(int id, bool save = true);

        Task<int> DeleteAsync(int id, bool save = true);

        #endregion delete
    }
}