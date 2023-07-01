using AppCommon;
using AppCommon.DataLayer.DataMain.Models;
using Microsoft.Extensions.Caching.Memory;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Telerik.DataSource.Extensions;

namespace AppCommon.Business
{
    public enum EnmCacheKey
    {
        WordList,
        Parameter,
        UserList,
        AuthorityList
    }

    public class CacheHelper : IDisposable
    {
        public readonly MainDataContext dataContext;
        private readonly IMemoryCache memoryCache;

        public CacheHelper(MainDataContext _dataContext, IMemoryCache _memoryCache)
        {
            this.dataContext = _dataContext;
            this.memoryCache = _memoryCache;
        }

        public static MemoryCacheEntryOptions GetOptions()
        {
            return new()
            {
                Priority = CacheItemPriority.NeverRemove,
                AbsoluteExpiration = DateTime.Now.AddMinutes(60), //memoride kalma süresi, süre bitince memoryden silinir
                SlidingExpiration = TimeSpan.FromMinutes(15), //her erişimde AbsoluteExp. e SlidingExp. kadar zaman ekler
            };
        }

        public Parameter GetParameter()
        {
            if (memoryCache.TryGetValue(EnmCacheKey.Parameter, out Parameter parameterCache))
            {
                return parameterCache;
            }
            else
            {
                var parameter = this.dataContext.Parameter.First();

                memoryCache.Set(EnmCacheKey.Parameter, parameter, CacheHelper.GetOptions());

                return parameter;
            }
        }

        public List<User> GetUserList()
        {
            if (memoryCache.TryGetValue(EnmCacheKey.UserList, out List<User> userListCache))
            {
                return userListCache;
            }
            else
            {
                var userList = this.dataContext.User.ToList();

                memoryCache.Set(EnmCacheKey.UserList, userList, CacheHelper.GetOptions());

                return userList;
            }
        }

        public void Remove(EnmCacheKey cacheKey)
        {
            this.memoryCache?.Remove(cacheKey);
        }

        public object? Exist(EnmCacheKey cacheKey)
        {
            return this.memoryCache?.Get(cacheKey);
        }

        public object? Exist(string cacheKey)
        {
            return this.memoryCache?.Get(cacheKey);
        }

        public T Get<T>(EnmCacheKey cacheKey)
        {
            return this.memoryCache.Get<T>(cacheKey);
        }

        public T Set<T>(EnmCacheKey cacheKey, T item)
        {
            return this.memoryCache.Set<T>(cacheKey, item, CacheHelper.GetOptions());
        }

        public List<MoWord> GetDictionary(string? rootPath)
        {
            List<MoWord> wordList = new();
            string cacheKey = EnmCacheKey.WordList.ToString();
            //wordlist yükleniyor
            if (this.Exist(cacheKey) == null)
            {
                List<FileInfo> files = new();
                var dirDictionary = new DirectoryInfo($"{rootPath}\\client\\dictionary");
                files.AddRange(dirDictionary.EnumerateFiles("*" + ".json", SearchOption.TopDirectoryOnly));

                var dirViewDictionary = new DirectoryInfo($"{rootPath}\\client\\views");
                files.AddRange(dirViewDictionary.EnumerateFiles("*" + ".json", SearchOption.AllDirectories));

                foreach (var file in files)
                {
                    var jsonText = File.ReadAllText(file.FullName, Encoding.UTF8);
                    var words = JsonConvert.DeserializeObject<List<MoWord>>(jsonText);
                    if (words != null)
                    {
                        wordList.AddRange(words);
                    }
                }
                this.memoryCache.Set(cacheKey, wordList, CacheHelper.GetOptions());
            }

            return this.memoryCache.Get<List<MoWord>>(cacheKey);
        }


        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }
    }
}
