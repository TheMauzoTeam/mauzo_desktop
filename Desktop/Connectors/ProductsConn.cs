﻿/*
 * MIT License
 *
 * Copyright (c) 2020 The Mauzo Team
 *
 * Permission is hereby granted, free of charge, to any person obtaining a copy
 * of this software and associated documentation files (the "Software"), to deal
 * in the Software without restriction, including without limitation the rights
 * to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
 * copies of the Software, and to permit persons to whom the Software is
 * furnished to do so, subject to the following conditions:
 *
 * The above copyright notice and this permission notice shall be included in all
 * copies or substantial portions of the Software.
 *
 * THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
 * IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
 * FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
 * AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
 * LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
 * OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
 * SOFTWARE.
 */
using Desktop.Properties;
using Desktop.Templates;
using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;

namespace Desktop.Connectors
{

    /// <summary>
    /// Esta clase gestiona las peticiones al servidor de la clase Products
    /// </summary>
    /// <remarks>@lluminar - Lidia Martínez</remarks>
    class ProductsConn
    {
        private readonly string mauzoUrl = Settings.Default.MauzoServer + "/api/products";
        private string token = LoginConn.Token;

        /// <summary>
        /// Inicia la conexión y nos devuelve una lista de productos
        /// </summary>
        public List<Product> List
        {
            get
            {
                // Iniciamos la conexión.
                Uri baseUrl = new Uri(mauzoUrl + "/");
                IRestClient client = new RestClient(baseUrl);
                IRestRequest request = new RestRequest(Method.GET);

                // Agregamos la autorización de token en el header.
                request.AddHeader("Authorization", token);
                
                // Ejecutamos la petición.
                IRestResponse response = client.Execute(request);

                // Inicializamos la lista de productos
                List<Product> products = null;

                // Procesamos el objeto del producto
                if (response.IsSuccessful)
                    products = JsonConvert.DeserializeObject<List<Product>>(response.Content);
                else
                    LoginConn.CalculateException(response, "No se ha encontrado el producto");

                // Devolvemos el objeto
                return products;
            }
        }

        /// <summary>
        /// Nos añade un producto con todos sus atributos a la base de datos, en caso de no poder nos manda un mensaje de error
        /// </summary>
        /// <param name="product">El producto a añadir a la base de datos</param>
        public void Add(Product product)
        {
            // Iniciamos la conexión.
            Uri baseUrl = new Uri(mauzoUrl + "/");
            IRestClient client = new RestClient(baseUrl);
            IRestRequest request = new RestRequest(Method.POST);

            // Agregamos la autorización de token en el header.
            request.AddHeader("Authorization", token);

            // Convertimos el producto a json y lo incorporamos a la petición.
            string jsonRequest = JsonConvert.SerializeObject(new
            {
                prodName = product.ProdName,
                prodCode = product.ProdCode,
                prodPrice = product.ProdPrice,
                prodDesc = product.ProdDesc,
                prodPic = product.ProdPic
            });

            // Se añade el json al cuerpo de la petición.
            request.AddJsonBody(jsonRequest);

            // Ejecutamos la petición.
            IRestResponse response = client.Execute(request);

            // Procesamos la respuesta de la petición.
            if (!response.IsSuccessful)
                LoginConn.CalculateException(response, "No se ha encontrado el producto");
        }

        /// <summary>
        /// Este método nos devuelve un producto a partir de un id
        /// </summary>
        /// <param name="id"> Atributo a partir del cual conseguimos el objeto </param>
        /// <returns> El producto obtenido </returns>
        public Product Get(int id)
        {
            // Iniciamos la conexión.
            Uri baseUrl = new Uri(mauzoUrl + "/" + id);
            IRestClient client = new RestClient(baseUrl);
            IRestRequest request = new RestRequest(Method.GET);

            // Agregamos la autorización de token en el header.
            request.AddHeader("Authorization", token);

            // Ejecutamos la petición.
            IRestResponse response = client.Execute(request);

            // Inicializamos el producto
            Product product = null;

            // Procesamos el objeto del producto
            if (response.IsSuccessful)
                product = JsonConvert.DeserializeObject<Product>(response.Content);
            else
                LoginConn.CalculateException(response, "No se ha encontrado el producto.");

            // Devolvemos el objeto
            return product;
        }

        /// <summary>
        /// Nos permite modificar un producto de la base de datos
        /// </summary>
        /// <param name="product"> El producto que vamos a modificar </param>
        public void Modify(Product product)
        {
            // Iniciamos la conexión.
            Uri baseUrl = new Uri(mauzoUrl + "/" + product);
            IRestClient client = new RestClient(baseUrl);
            IRestRequest request = new RestRequest(Method.PUT);

            // Agregamos la autorización de token en el header.
            request.AddHeader("Authorization", token);

            // Convertimos el producto a json y lo incorporamos a la petición.
            string jsonRequest = JsonConvert.SerializeObject(new
            {
                prodName = product.ProdName,
                prodCode = product.ProdCode,
                prodprice = product.ProdPrice,
                prodDesc = product.ProdDesc,
                prodPic = product.ProdPic
            });

            // Se añade el json al cuerpo de la petición.
            request.AddJsonBody(jsonRequest);

            // Ejecutamos la petición.
            IRestResponse response = client.Execute(request);

            // Procesamos la respuesta de la petición.
            if (!response.IsSuccessful)
                LoginConn.CalculateException(response, "No se ha encontrado el producto");
        }

        /// <summary>
        /// Método para eliminar un producto de la base de datos.
        /// </summary>
        /// <param name="product"> El producto a eliminar </param>
        public void Delete(Product product)
        {
            // Iniciamos la conexión.
            Uri baseUrl = new Uri(mauzoUrl + "/" + product.Id);
            IRestClient client = new RestClient(baseUrl);
            IRestRequest request = new RestRequest(Method.DELETE);

            // Agregamos la autorización de token en el header.
            request.AddHeader("Authorization", token);

            // Ejecutamos la petición.
            IRestResponse response = client.Execute(request);

            // Procesamos la respuesta de la petición.
            if (!response.IsSuccessful)
                LoginConn.CalculateException(response, "No se ha encontrado el producto");
        }
    }
}