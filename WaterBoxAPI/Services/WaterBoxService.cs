namespace WaterBoxAPI.Services;

public class WaterBoxService
{
    public static IDictionary<string, string> VerifySensors(int s1, int s2, int s3, int s4)
    {
        int v1Status = 0;
        int b1Status = 0;

        var c1 = new { s1 = Convert.ToBoolean(s1), s2 = Convert.ToBoolean(s2) };
        var c2 = new { s3 = Convert.ToBoolean(s3), s4 = Convert.ToBoolean(s4) };

        // If first element of objects is full filled and second is empty should be throwns an exception
        var c1LittleWater = c1.s2 && !c1.s1 || !c1.s1 && !c1.s2;
        var c1FullWater = c1.s1 && c1.s2;        

        var c2LittleWater = c2.s4 && !c2.s3 || !c2.s3 && !c2.s4;
        var c2FullWater = c2.s3 && c2.s4;        

        var c1ErrorCase = c1.s1 && !c1.s2;
        var c2ErrorCase = c2.s3 && !c2.s4;

        if (c1ErrorCase)
        {
            throw new Exception("Não é possível manter o s1 com água e o s2 sem água.");
        }
        else if (c2ErrorCase)
        {
            throw new Exception("Não é possível manter o s3 com água e o s4 sem água.");
        }
        else if (c1FullWater && c2LittleWater)
        {
            v1Status = 0;
            b1Status = 1;
        }
        else if (c1LittleWater && c2FullWater)
        {
            v1Status = 1;
            b1Status = 0;
        }
        else if (c1FullWater && c2FullWater)
        {
            v1Status = 0;
            b1Status = 0;
        }
        else if (c1LittleWater && c2LittleWater)
        {            
            v1Status = 1;
            b1Status = 0;
        }        

        return new Dictionary<string, string>() 
        {
            { "v1", v1Status == 1 ? "Ativado" : "Desativado" },
            { "b1", b1Status == 1 ? "Ativado" : "Desativado"}
        };
    }
}
