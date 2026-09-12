namespace MotoPOS.API.Constants;

public static class ValidationMessages
{
    public static class Productos
    {
        public const string CodigoRequerido = "El código del producto es obligatorio.";
        public const string CodigoMaximo = "El código del producto no puede exceder los 50 caracteres.";
        public const string NombreRequerido = "El nombre del producto es obligatorio.";
        public const string NombreMaximo = "El nombre del producto no puede exceder los 150 caracteres.";
        public const string MarcaIdRequerido = "El ID de la marca del producto es obligatorio.";
        public const string CategoriaIdRequerido = "El ID de la categoría del producto es obligatorio.";
        public const string PrecioCompraRequerido = "El precio de compra del producto es obligatorio.";
        public const string PrecioCompraPositivo = "El precio de compra del producto debe ser un valor positivo.";
        public const string PrecioVentaRequerido = "El precio de venta del producto es obligatorio.";
        public const string PrecioVentaPositivo = "El precio de venta del producto debe ser un valor positivo.";
        public const string StockMinimoRequerido = "El stock mínimo del producto es obligatorio.";
        public const string StockMinimoNoNegativo = "El stock mínimo del producto no debe ser negativo.";
    }
    public static class Clientes
    {
        public const string NitRequerido = "El NIT del cliente es obligatorio.";
        public const string NitMaximo = "El NIT del cliente no puede exceder los 20 caracteres.";
        public const string NombreRequerido = "El nombre del cliente es obligatorio.";
        public const string NombreMaximo = "El nombre del cliente no puede exceder los 200 caracteres.";
        public const string DireccionMaximo = "La dirección del cliente no puede exceder los 300 caracteres.";
        public const string TelefonoMaximo = "El teléfono del cliente no puede exceder los 20 caracteres.";
        public const string CorreoMaximo = "El correo del cliente no puede exceder los 100 caracteres.";
        public const string CorreoFormato = "El formato del correo electrónico no es válido.";
    }
    public static class Proveedores
    {
        public const string NitRequerido = "El NIT del proveedor es obligatorio.";
        public const string NitMaximo = "El NIT del proveedor no puede exceder los 20 caracteres.";
        public const string NombreEmpresaRequerido = "El nombre de la empresa es obligatorio.";
        public const string NombreEmpresaMaximo = "El nombre de la empresa no puede exceder los 200 caracteres.";
        public const string NombreContactoMaximo = "El nombre del contacto no puede exceder los 100 caracteres.";
        public const string DireccionMaximo = "La dirección del proveedor no puede exceder los 300 caracteres.";
        public const string TelefonoMaximo = "El teléfono del proveedor no puede exceder los 20 caracteres.";
        public const string CorreoMaximo = "El correo del proveedor no puede exceder los 100 caracteres.";
        public const string CorreoFormato = "El formato del correo electrónico no es válido.";
    }
    public static class Marcas
    {
        public const string NombreRequerido = "El nombre de la marca es obligatorio.";
        public const string NombreMaximo = "El nombre de la marca no puede exceder los 100 caracteres.";
    }
    public static class Categorias
    {
        public const string NombreRequerido = "El nombre de la categoría es obligatorio.";
        public const string NombreMaximo = "El nombre de la categoría no puede exceder los 100 caracteres.";
    }
    public static class Usuarios
    {
        public const string NombreRequerido = "El nombre del usuario es obligatorio.";
        public const string NombreMaximo = "El nombre del usuario no puede exceder los 100 caracteres.";
        public const string UsuarioLoginRequerido = "El nombre de usuario es obligatorio.";
        public const string UsuarioLoginMaximo = "El nombre de usuario no puede exceder los 50 caracteres.";
        public const string ContrasenaRequerida = "La contraseña es obligatoria.";
        public const string ContrasenaMinima = "La contraseña debe tener al menos 6 caracteres.";
        public const string RolIdRequerido = "El ID del rol es obligatorio.";
    }
}
public static class ErrorMessages
{
    public static class Productos
    {
        public const string CodigoDuplicado = "Ya existe un producto con el mismo código.";
        public const string MarcaNoExiste = "No existe la marca especificada.";
        public const string CategoriaNoExiste = "No existe la categoría especificada.";
        public const string ProductoNoEncontrado = "No existe el producto con el ID especificado.";
        public const string ProductoNoRecuperado = "No se pudo recuperar el producto creado.";
        public const string ProductoInactivo = "El prodicto esta inactivo";
        public const string StockInsuficiente = "El stock es insuficiente para cubrir la cantidad solicitada";
    }
    public static class Clientes
    {
        public const string ClienteNoEncontrado = "No existe el cliente con el ID especificado.";
        public const string NitDuplicado = "Ya existe un cliente con el mismo NIT.";
        public const string ClienteNoRecuperado = "No se pudo recuperar el cliente creado.";
    }
    public static class Proveedores
    {
        public const string ProveedorNoEncontrado = "No existe el proveedor con el ID especificado.";
        public const string NitDuplicado = "Ya existe un proveedor con el mismo NIT.";
        public const string ProveedorNoRecuperado = "No se pudo recuperar el proveedor creado.";
    }
    public static class Marcas
    {
        public const string MarcaNoEncontrada = "No existe la marca con el ID especificado.";
        public const string NombreDuplicado = "Ya existe una marca con el mismo nombre.";
        public const string MarcaNoRecuperada = "No se pudo recuperar la marca creada.";
    }
    public static class Categorias
    {
        public const string CategoriaNoEncontrada = "No existe la categoría con el ID especificado.";
        public const string NombreDuplicado = "Ya existe una categoría con el mismo nombre.";
        public const string CategoriaNoRecuperada = "No se pudo recuperar la categoría creada.";
    }
    public static class Usuarios
    {
        public const string UsuarioNoEncontrado = "No existe el usuario con el ID especificado.";
        public const string UsuarioLoginDuplicado = "Ya existe un usuario con el mismo nombre de usuario.";
        public const string UsuarioNoRecuperado = "No se pudo recuperar el usuario creado.";
        public const string RolNoExiste = "No existe el rol especificado.";

    }
    public static class Ventas
    {
        public const string VentaNoEncontrada = "No se encontro la venta especificada";
    }
    public static class Compras
    {
        public const string CompraNoEncontrada = "No se encontro la compra especificada";
    }
    public static class MovimientosInventario
    {
        public const string MovimientoNoEncontrado = "No se encontro el movimiento de inventario especificado";
    }
}