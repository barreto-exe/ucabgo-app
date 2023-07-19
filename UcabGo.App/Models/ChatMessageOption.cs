namespace UcabGo.App.Models
{
    public class ChatMessageOption
    {
        public required string OptionName { get; set; }
        public required string FirstPortion { get; set; }
        public required IEnumerable<string> FinalPortions { get; set; }

        public static IEnumerable<ChatMessageOption> GetChatMessageOptions() => new List<ChatMessageOption>
        {
            new()
            {
                OptionName = "Saludar",
                FirstPortion = "Hola,",
                FinalPortions = new List<string>
                {
                    "Buenos días",
                    "Buenas tardes",
                    "Buenas noches",
                    "Gracias por aceptar mi solicitud",
                    "¿Dónde nos encontramos?",
                    "¿Cuál es el punto de encuentro?",
                }
            },
            new()
            {
                OptionName = "Alertas",
                FirstPortion = string.Empty,
                FinalPortions = new List<string>
                {
                    "Estoy en el lugar de encuentro",
                    "Estoy saliendo",
                    "Estoy en camino",
                    "Estoy llegando",
                    "¡Ya llegué!",
                    "Estoy esperando",
                }
            },
            new()
            {
                OptionName = "Puntos de encuentro UCAB",
                FirstPortion = "Vamos a encontrarnos en",
                FinalPortions = new List<string>
                {
                    "El pide-cola",
                    "La plaza de la biblioteca central",
                    "El estacionamiento de visitantes",
                    "La entrada de visitantes",
                    "Caja negra",
                    "La entrada superior del Loyola",
                    "Las canchas de volley",
                    "La escuela de informática",
                    "La escuela de comunicación social",
                    "La escuela de industrial",
                }
            },
            new()
            {
                OptionName = "Puntos de encuentro externos",
                FirstPortion = "Vamos a encontrarnos en",
                FinalPortions = new List<string>
                {
                    "La entrada de la urbanización",
                    "La entrada del centro comercial",
                    "La entrada del supermercado",
                    "La vigilancia del lugar",
                    "La avenida, afuera",
                }
            },
            new()
            {
                OptionName = "Te espero en",
                FirstPortion = "Te espero en",
                FinalPortions = new List<string>
                {
                    "La entrada de la urbanización",
                    "La entrada del centro comercial",
                    "La entrada del supermercado",
                    "La vigilancia del lugar",
                    "La avenida, afuera",
                }
            },
            new()
            {
                OptionName = "Espérame en",
                FirstPortion = "Espérame en",
                FinalPortions = new List<string>
                {
                    "La entrada de la urbanización",
                    "La entrada del centro comercial",
                    "La entrada del supermercado",
                    "La vigilancia del lugar",
                    "La avenida, afuera",
                }
            },
        };
    }
}
