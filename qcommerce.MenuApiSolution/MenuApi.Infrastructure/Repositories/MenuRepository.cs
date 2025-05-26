using ecommerce.SharedLibrary.logs;
using ecommerce.SharedLibrary.response;
using MenuApi.Application.DTOs.Mapper;
using MenuApi.Application.Interface;
using MenuApi.Domain.Entities;
using MenuApi.Infrastructure.Data;
using Microsoft.AspNetCore.Localization;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace MenuApi.Infrastructure.Repositories
{
    public class MenuRepository : IMenu
    {
        private readonly MenuDbContext _db;

        public MenuRepository(MenuDbContext db)
        {
            _db = db;
        }
        public async Task<Response> CreateAsync(Menu entity)
        {
            try
            {
                var getMenu = await GetByAsync(m => m.Name == entity.Name);
                if (getMenu is not null)
                    return new Response(false, $"{entity.Name} already added");
                var currentEntity = _db.Menus.Add(entity).Entity;
                await _db.SaveChangesAsync();
                if (currentEntity.Id > 0)
                    return new Response(true, $"{entity.Name} added to database successfully");
                else
                    return new Response(false, "Error occurred while adding menu");
            }
            catch (Exception ex)
            {
                LogException.LogExceptions(ex);

                return new Response(false, "Error occurred while adding menu");
            }
        }

        public async Task<Response> DeleteAsync(Menu entity)
        {
            try
            {
                var menu = await FindByIdAsync(entity.Id);
                if (menu == null)
                    return new Response(false, $"{entity.Name} not found");
                _db.Menus.Remove(menu);
                await _db.SaveChangesAsync();
                return new Response(true, $"{entity.Name} deleted successfully");
            }
            catch(Exception ex) 
            {
                LogException.LogExceptions(ex);

                return new Response(false, "Error occurred while adding menu");
            }
            
        }

        public async Task<Menu> FindByIdAsync(int id)
        {
            
                var getMenu = await _db.Menus.FindAsync(id);
                return getMenu is not null ? getMenu : null!;               
            
            //catch(Exception ex)
            //{
            //    LogException.LogExceptions(ex);

            //    throw new InvalidOperationException("Error occurred while retrieving menu");
            //}


            
        }

        public async Task<IEnumerable<Menu>> GetAllAsync()
        {
            try
            {
                var listOfMenu = await _db.Menus.ToListAsync();
                return listOfMenu != null ? listOfMenu : Enumerable.Empty<Menu>();
            }
            catch(Exception ex)
            {
                LogException.LogExceptions(ex);

               Console.Write("Error occurred while retrieving menu");
                return null;
            }
        }

        public async Task<Menu> GetByAsync(Expression<Func<Menu, bool>> predicate)
        {
            try
            {
                var menu = await _db.Menus.Where(predicate).FirstOrDefaultAsync();
                return menu is not null ? menu : null!;
            }
            catch (Exception ex)
            {
                LogException.LogExceptions(ex);

                throw new InvalidOperationException("Error occurred while retrieving menu");
            }
        }

        public async Task<Response> UpdateAsync(Menu entity)
        {
            var getMenu = await FindByIdAsync(entity.Id);
            if (getMenu == null)
                return new Response(false, $"Pizza not found");
            _db.Entry(entity).State = EntityState.Detached;
            _db.Menus.Update(entity);
            await _db.SaveChangesAsync();
            return new Response(true, $"{entity.Name} is updated successfully");
        }
    }
}
