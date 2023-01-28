using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Infrastructure.Data;
using Core.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Core.Interfaces;
using Core.Specifications;
using API.Dtos;
using AutoMapper;

namespace API.Controllers
{

    [ApiController]
    [Route("api/[controller]")]
    public class ProductsController : ControllerBase
    {
       // private readonly StoreContext _context;
        public readonly IProductRepository _repo;

        private readonly IGenericRepository<Product> _productsRepo;
        private readonly IGenericRepository<ProductBrand> _productBrandRepo;

        private readonly IGenericRepository<ProductType> _productTypeRepo;

        private readonly IMapper _mapper;

        // public ProductsController(IProductRepository repo)
        // {
        //     _repo = repo;
        //    // _context = context;
        // }

        public ProductsController(IGenericRepository<Product> productsRepo, IGenericRepository<ProductBrand> productBrandRepo,
         IGenericRepository<ProductType> productTypeRepo, IMapper mapper){
            _productsRepo = productsRepo;
            _productBrandRepo = productBrandRepo;
            _productTypeRepo = productTypeRepo;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<IReadOnlyList<ProductToReturnDto>>> GetProducts()
        {
            var spec = new ProductsWithTypesAndBrandsSpecification();

            //generic Repo
            var products = await _productsRepo.ListAsync(spec);

            //Repository Pattern
           // var products = await _repo.GetProductsAsync();

            //************* without Auto Mapper ******************
            // return products.Select(product => new ProductToReturnDto{
            //         Id = product.Id,
            //         Name = product.Name,
            //         Description = product.Description,
            //         PictureUrl = product.PictureUrl,
            //         Price = product.Price,
            //         ProductBrand = product.ProductBrand.Name,
            //         ProductType = product.ProductType.Name
            // }).ToList();

            //*********** with Auto Mapper ************
            return Ok(_mapper.Map<IReadOnlyList<Product>, IReadOnlyList<ProductToReturnDto>>(products));

        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ProductToReturnDto>> GetProduct(int id)
        {

            var spec = new ProductsWithTypesAndBrandsSpecification(id);
            //return await _context.Products.FindAsync(id);

            //Repository Pattern
          //  return await _repo.GetProductByIdAsync(id);

            //generic Repo
           var product =  await _productsRepo.GetEntityWithSpec(spec);


        //without AutoMapper
        //    return new ProductToReturnDto{
        //     Id = product.Id,
        //     Name = product.Name,
        //     Description = product.Description,
        //     PictureUrl = product.PictureUrl,
        //     Price = product.Price,
        //     ProductBrand = product.ProductBrand.Name,
        //     ProductType = product.ProductType.Name
        //    };
        

        //after AutoMapper

        return _mapper.Map<Product, ProductToReturnDto>(product);
        }

        [HttpGet("brands")]
        public async Task<ActionResult<IReadOnlyList<ProductBrand>>> GetProductBrands() {
            

            //repository pattern
            //return Ok(await _repo.GetProductBrandsAsync());


            //generic repo
            return Ok(await _productBrandRepo.ListAllAsync());
        }

          [HttpGet("types")]
        public async Task<ActionResult<IReadOnlyList<ProductType>>> GetProductTypes() {

            //repository pattern
            //return Ok(await _repo.GetProductTypesAsync());


            //generic Repo
            return Ok(await _productTypeRepo.ListAllAsync());
        }

    }
}