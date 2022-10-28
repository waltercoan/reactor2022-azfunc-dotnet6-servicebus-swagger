using System.Collections.Generic;

namespace azfunc.Model;

public class MusicData
{
    private static ICollection<Music> musics;
    public static ICollection<Music> GetMusicData() {
        if(musics == null)
        {
            musics = new List<Music>(){
                new Music(){
                    id = 1,
                    name = "Hotel California",
                    artist =  "Eagles",	
                    year = 1976
                },
                new Music(){
                    id = 2,
                    name = "Stairway to Heaven",
                    artist =  "Led Zeppelin",	
                    year = 1971
                },
                new Music(){
                    id = 3,
                    name = "Living on the Edge",
                    artist =  "Aerosmith",	
                    year = 1993
                },
                new Music(){
                    id = 4,
                    name = "Amazing",
                    artist =  "Aerosmith",	
                    year = 1993
                },
                new Music(){
                    id = 5,
                    name = "Run to the Hills",
                    artist =  "Iron Maiden",	
                    year = 1982
                }
            };
        }
        return musics;
    }
}