using System.Collections.Generic;

public class Menu
{
    public Dictionary<int, Producto> Productos { get; set; }

    public Menu()
    {
        Productos = new Dictionary<int, Producto>();
    }

    public void AgregarProducto(Producto producto)
    {
        if (!Productos.ContainsKey(producto.Id))
        {
            Productos.Add(producto.Id, producto);
        }
    }

    public Producto ObtenerProducto(int id)
    {
        Productos.TryGetValue(id, out var producto);
        return producto;
    }

    public void EliminarProducto(int id)
    {
        if (Productos.ContainsKey(id))
        {
            Productos.Remove(id);
        }
    }
}
