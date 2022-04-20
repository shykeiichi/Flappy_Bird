using Fjord.Modules.Game;

namespace Flappy_Bord {
    class bird : entity {
        public bird() {
            use(new bird_controller(), this);
        }
    }
}