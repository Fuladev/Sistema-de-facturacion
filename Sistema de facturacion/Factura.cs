using System;
using System.Collections.Generic;
using System.Linq;

public class Factura
{
    public List<Producto> Productos { get; set; }
    public decimal Total { get; set; }
    public int NumeroDeMesa { get; set; }

    public Factura(List<Producto> productos, int numeroDeMesa)
    {
        Productos = productos;
        NumeroDeMesa = numeroDeMesa;
        Total = CalcularTotal();
    }

    private decimal CalcularTotal()
    {
        return Productos.Sum(producto => producto.Precio * producto.Cantidad);
    }

    public void ImprimirFactura()
    {
        Console.WriteLine($"Factura de la Mesa {NumeroDeMesa}:");
        Console.WriteLine("----------------------------");
        foreach (var producto in Productos)
        {
            Console.WriteLine($"{producto.Nombre} - Cantidad: {producto.Cantidad} - Precio Unitario: {producto.Precio:C} - Subtotal: {producto.Precio * producto.Cantidad:C}");
        }
        Console.WriteLine("----------------------------");
        Console.WriteLine($"Total: {Total:C}");
    }
}

