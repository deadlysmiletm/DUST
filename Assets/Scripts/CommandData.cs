public class CommandData{
    //Aquí se almacenan los datos del Diccionario para la consola.
    public Console.ConsoleCommand command;
    public string name;
    public string description;

    //Se sobreescribe ToString para simplificar el código en la consola.
    public override string ToString()
    {
        return "[" + name + " => " + description + "]";
    }

}
