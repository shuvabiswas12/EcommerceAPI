﻿using AutoMapper;
using EcommerceAPI.Data.UnitOfWork;
using EcommerceAPI.Domain;
using EcommerceAPI.DTOs;
using EcommerceAPI.DTOs.GenericResponse;
using EcommerceAPI.Services.IServices;
using EcommerceAPI.Utilities.Exceptions;
using Microsoft.AspNetCore.Http.HttpResults;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EcommerceAPI.Services.Services
{
    public class CategoryServices : ICategoryServices
    {
        private readonly IUnitOfWork _unitOfWork;

        public CategoryServices(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task DeleteCategory(object id)
        {
            if (id == null)
            {
                throw new ArgumentNullException(nameof(id), "Please provide a Category ID.");
            }
            var dataToDelete = await _unitOfWork.GenericRepository<Category>().GetTAsync(predicate: x => x.Id == id.ToString());
            if (dataToDelete == null)
            {
                throw new ApiException(System.Net.HttpStatusCode.NotFound, "");
            }
            await _unitOfWork.GenericRepository<Category>().DeleteAsync(dataToDelete);
            await _unitOfWork.SaveAsync();
        }

        public async Task<IEnumerable<Category>> GetAllCategories()
        {
            return await _unitOfWork.GenericRepository<Category>().GetAllAsync(orderBy: x => x.OrderByDescending(x => x.CreatedAt));
        }

        public async Task<Category> GetCategoryById(object id)
        {
            if (id == null)
            {
                throw new ArgumentNullException(nameof(id), "Please provide a Category ID.");
            }
            return await _unitOfWork.GenericRepository<Category>().GetTAsync(predicate: x => x.Id == id.ToString());
        }

        public async Task UpdateCategory(object id, string categoryName)
        {
            if (id == null || string.IsNullOrWhiteSpace(categoryName))
            {
                throw new ArgumentNullException("ID and category name must be provided.");
            }
            var dataToUpdate = await _unitOfWork.GenericRepository<Category>().GetTAsync(predicate: x => x.Id == id.ToString());
            if (dataToUpdate == null)
            {
                throw new ApiException(System.Net.HttpStatusCode.NotFound, "");
            }
            dataToUpdate.Name = categoryName;
            await _unitOfWork.SaveAsync();
        }

        public async Task<string> CreateCategory(CategoryCreateDTO createCategory)
        {
            Category category = new Category { Name = createCategory.Name.Trim() };
            var createdCategory = await _unitOfWork.GenericRepository<Category>().AddAsync(category);
            await _unitOfWork.SaveAsync();
            return createdCategory.Id;
        }
    }
}
