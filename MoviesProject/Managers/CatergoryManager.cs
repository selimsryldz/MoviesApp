using MoviesProject.Database;
using MoviesProject.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MoviesProject.Managers
{
    public class CatergoryManager
    {
        private Repository<Categories> repo_category = new Repository<Categories>();
        public List<Categories> GetCategories()
        {
            return repo_category.List();
        }
        public Categories GetCategoryById(int id)
        {
            return repo_category.Find(x => x.Id == id);
        }
    }
}