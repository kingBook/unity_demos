using System.Collections.Generic;

namespace SwfParserRuntime {

    public interface ICharacterIdTag {

        void FindUsedCharacterIds(List<ushort> characterIds, Swf swf);

        ushort GetCharacterId();

    }
}