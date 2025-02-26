using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

public class MainClass
{
    private static Dictionary<int, Mesa> mesas = new Dictionary<int, Mesa>();
    private static Menu menu = new Menu();
    private const string FacturasFilePath = "facturas.txt";
    private const string MenuFilePath = "menu.txt";

    public static void Main(string[] args)
    {
        CargarMenuDesdeArchivo();

        while (true)
        {
            Console.WriteLine("Seleccione una opción:");
            Console.WriteLine("1. Seleccionar Mesa");
            Console.WriteLine("2. Editar Menu");
            Console.WriteLine("3. Imprimir Menu");
            Console.WriteLine("4. Cierre de Caja");
            Console.WriteLine("5. Salir");

            var opcion = Console.ReadLine();

            switch (opcion)
            {
                case "1":
                    SubMenuMesa();
                    break;
                case "2":
                    SubMenuEditarMenu();
                    break;
                case "3":
                    ImprimirMenu();
                    break;
                case "4":
                    CierreDeCaja();
                    break;
                case "5":
                    return;
                default:
                    Console.WriteLine("Opción no válida.");
                    break;
            }
        }
    }

    private static void SubMenuMesa()
    {
        Console.WriteLine("Ingrese el número de mesa:");
        if (int.TryParse(Console.ReadLine(), out int numeroDeMesa))
        {
            if (!mesas.ContainsKey(numeroDeMesa))
            {
                mesas[numeroDeMesa] = new Mesa(numeroDeMesa);
            }

            while (true)
            {
                Console.WriteLine("Seleccione una opción:");
                Console.WriteLine("1. Agregar Producto");
                Console.WriteLine("2. Editar Producto");
                Console.WriteLine("3. Facturar");
                Console.WriteLine("4. Volver");

                var opcion = Console.ReadLine();

                switch (opcion)
                {
                    case "1":
                        AgregarProductoAMesa(numeroDeMesa);
                        break;
                    case "2":
                        EditarProductoEnMesa(numeroDeMesa);
                        break;
                    case "3":
                        FacturarMesa(numeroDeMesa);
                        return;
                    case "4":
                        return;
                    default:
                        Console.WriteLine("Opción no válida.");
                        break;
                }
            }
        }
        else
        {
            Console.WriteLine("Número de mesa no válido.");
        }
    }

    private static void SubMenuEditarMenu()
    {
        while (true)
        {
            Console.WriteLine("Seleccione una opción:");
            Console.WriteLine("1. Agregar Producto");
            Console.WriteLine("2. Eliminar Producto");
            Console.WriteLine("3. Buscar Producto");
            Console.WriteLine("4. Volver");

            var opcion = Console.ReadLine();

            switch (opcion)
            {
                case "1":
                    AgregarProductoAlMenu();
                    break;
                case "2":
                    EliminarProductoDelMenu();
                    break;
                case "3":
                    BuscarProductoEnMenu();
                    break;
                case "4":
                    return;
                default:
                    Console.WriteLine("Opción no válida.");
                    break;
            }
        }
    }

    private static void AgregarProductoAMesa(int numeroDeMesa)
    {
        Console.WriteLine("Ingrese el ID del producto:");
        if (int.TryParse(Console.ReadLine(), out int id))
        {
            var producto = menu.ObtenerProducto(id);
            if (producto != null)
            {
                Console.WriteLine("Ingrese la cantidad del producto:");
                if (int.TryParse(Console.ReadLine(), out int cantidad) && cantidad > 0)
                {
                    var productoConCantidad = new Producto
                    {
                        Id = producto.Id,
                        Nombre = producto.Nombre,
                        Precio = producto.Precio,
                        Cantidad = cantidad
                    };
                    mesas[numeroDeMesa].AgregarProducto(productoConCantidad);
                    Console.WriteLine($"{cantidad} unidades del producto agregado a la mesa.");
                }
                else
                {
                    Console.WriteLine("Cantidad no válida.");
                }
            }
            else
            {
                Console.WriteLine("Producto no encontrado en el menú.");
            }
        }
        else
        {
            Console.WriteLine("ID no válido.");
        }
    }

    private static void EditarProductoEnMesa(int numeroDeMesa)
    {
        var mesa = mesas[numeroDeMesa];
        Console.WriteLine("Ingrese el ID del producto a editar:");
        if (int.TryParse(Console.ReadLine(), out int id))
        {
            var producto = mesa.Orden.FirstOrDefault(p => p.Id == id);
            if (producto != null)
            {
                Console.WriteLine("Seleccione una opción:");
                Console.WriteLine("1. Cambiar cantidad");
                Console.WriteLine("2. Eliminar producto");
                var opcion = Console.ReadLine();

                switch (opcion)
                {
                    case "1":
                        Console.WriteLine("Ingrese la nueva cantidad del producto:");
                        if (int.TryParse(Console.ReadLine(), out int nuevaCantidad) && nuevaCantidad > 0)
                        {
                            producto.Cantidad = nuevaCantidad;
                            Console.WriteLine("Cantidad actualizada.");
                        }
                        else
                        {
                            Console.WriteLine("Cantidad no válida.");
                        }
                        break;
                    case "2":
                        mesa.Orden.Remove(producto);
                        Console.WriteLine("Producto eliminado de la mesa.");
                        break;
                    default:
                        Console.WriteLine("Opción no válida.");
                        break;
                }
            }
            else
            {
                Console.WriteLine("Producto no encontrado en la mesa.");
            }
        }
        else
        {
            Console.WriteLine("ID no válido.");
        }
    }

    private static void FacturarMesa(int numeroDeMesa)
    {
        var factura = new Factura(mesas[numeroDeMesa].Orden, numeroDeMesa);
        factura.ImprimirFactura();
        GuardarFactura(factura);
        mesas.Remove(numeroDeMesa);
    }

    private static void GuardarFactura(Factura factura)
    {
        using (StreamWriter writer = new StreamWriter(FacturasFilePath, true))
        {
            writer.WriteLine($"Factura de la Mesa {factura.NumeroDeMesa}:");
            writer.WriteLine("----------------------------");
            foreach (var producto in factura.Productos)
            {
                writer.WriteLine($"Producto: {producto.Nombre} - Cantidad: {producto.Cantidad} - Precio Unitario: {producto.Precio:C} - Subtotal: {producto.Precio * producto.Cantidad:C}");
            }
            writer.WriteLine("----------------------------");
            writer.WriteLine($"Total: {factura.Total:C}");
            writer.WriteLine();
        }
    }

    private static void AgregarProductoAlMenu()
    {
        Console.WriteLine("Ingrese el ID del producto:");
        if (int.TryParse(Console.ReadLine(), out int id))
        {
            Console.WriteLine("Ingrese el nombre del producto:");
            var nombre = Console.ReadLine();
            Console.WriteLine("Ingrese el precio del producto:");
            if (decimal.TryParse(Console.ReadLine(), out decimal precio))
            {
                var producto = new Producto { Id = id, Nombre = nombre, Precio = precio, Cantidad = 1 };
                menu.AgregarProducto(producto);
                GuardarProductoEnArchivo(producto);
                Console.WriteLine("Producto agregado al menú.");
            }
            else
            {
                Console.WriteLine("Precio no válido.");
            }
        }
        else
        {
            Console.WriteLine("ID no válido.");
        }
    }

    private static void EliminarProductoDelMenu()
    {
        Console.WriteLine("Ingrese el ID del producto a eliminar:");
        if (int.TryParse(Console.ReadLine(), out int id))
        {
            menu.EliminarProducto(id);
            EliminarProductoDelArchivo(id);
            Console.WriteLine("Producto eliminado del menú.");
        }
        else
        {
            Console.WriteLine("ID no válido.");
        }
    }

    private static void BuscarProductoEnMenu()
    {
        Console.WriteLine("Ingrese el ID del producto a buscar:");
        if (int.TryParse(Console.ReadLine(), out int id))
        {
            var producto = menu.ObtenerProducto(id);
            if (producto != null)
            {
                Console.WriteLine($"Producto encontrado: {producto.Nombre} - Precio: {producto.Precio:C}");
            }
            else
            {
                Console.WriteLine("Producto no encontrado.");
            }
        }
        else
        {
            Console.WriteLine("ID no válido.");
        }
    }

    private static void ImprimirMenu()
    {
        Console.WriteLine("Menú:");
        Console.WriteLine("----------------------------");
        foreach (var producto in menu.Productos.Values)
        {
            Console.WriteLine($"ID: {producto.Id} - {producto.Nombre} - Precio: {producto.Precio:C}");
        }
        Console.WriteLine("----------------------------");
    }

    private static void CargarMenuDesdeArchivo()
    {
        if (File.Exists(MenuFilePath))
        {
            var lineas = File.ReadAllLines(MenuFilePath);
            foreach (var linea in lineas)
            {
                var partes = linea.Split(',');
                if (partes.Length == 3 &&
                    int.TryParse(partes[0], out int id) &&
                    decimal.TryParse(partes[2], out decimal precio))
                {
                    var producto = new Producto
                    {
                        Id = id,
                        Nombre = partes[1],
                        Precio = precio,
                        Cantidad = 1
                    };
                    menu.AgregarProducto(producto);
                }
            }
        }
    }

    private static void GuardarProductoEnArchivo(Producto producto)
    {
        using (StreamWriter writer = new StreamWriter(MenuFilePath, true))
        {
            writer.WriteLine($"{producto.Id},{producto.Nombre},{producto.Precio}");
        }
    }

    private static void EliminarProductoDelArchivo(int id)
    {
        if (File.Exists(MenuFilePath))
        {
            var lineas = File.ReadAllLines(MenuFilePath).ToList();
            var lineasActualizadas = lineas.Where(linea => !linea.StartsWith($"{id},")).ToList();
            File.WriteAllLines(MenuFilePath, lineasActualizadas);
        }
    }

    private static void CierreDeCaja()
    {
        if (File.Exists(FacturasFilePath))
        {
            var facturas = File.ReadAllText(FacturasFilePath);
            Console.WriteLine("Cierre de Caja:");
            Console.WriteLine("----------------------------");
            Console.WriteLine(facturas);
            Console.WriteLine("----------------------------");
        }
        else
        {
            Console.WriteLine("No hay facturas registradas.");
        }
    }
}
