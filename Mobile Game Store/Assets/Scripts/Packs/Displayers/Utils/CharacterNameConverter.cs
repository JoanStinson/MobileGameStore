namespace JGM.GameStore.Packs.Displayers.Utils
{
    public class CharacterNameConverter
    {
        public void GetCharacterNameFromId(in string characterId, out string characterName)
        {
            characterName = string.Empty;

            switch (characterId)
            {
                case "character_1":
                    characterName = "Sunna";
                    break;

                case "character_2":
                    characterName = "Hodur";
                    break;

                case "character_3":
                    characterName = "Lagnar";
                    break;
            }
        }
    }
}