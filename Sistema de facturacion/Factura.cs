using System;
using System.Collections.Generic;
using System.Linq;

public class Factura
{
    public List<Producto> Productos { get; set; }
    public decimal Total { get; set; }

    public Factura(List<Producto> productos)
    {
        Productos = productos;
        Total = CalcularTotal();
    }

    private decimal CalcularTotal()
    {
        return Productos.Sum(producto => producto.Precio);
    }

    public void ImprimirFactura()
    {
        Console.WriteLine("Factura:");
        Console.WriteLine("----------------------------");
        foreach (var producto in Productos)
        {
            Console.WriteLine($"Producto: {producto.Nombre} - Precio Unitario: {producto.Precio:C}");
        }
        Console.WriteLine("----------------------------");
        Console.WriteLine($"Total: {Total:C}");
    }
}

