using System.Collections.Generic;
using System.Linq;

public class Mesa
{
    public int NumeroDeMesa { get; set; }
    public List<Producto> Orden { get; set; }

    public Mesa(int numeroDeMesa)
    {
        NumeroDeMesa = numeroDeMesa;
        Orden = new List<Producto>();
    }

    public void AgregarProducto(Producto producto)
    {
        Orden.Add(producto);
    }

    public decimal CalcularTotal()
    {
        return Orden.Sum(producto => producto.Precio);
    }
}
