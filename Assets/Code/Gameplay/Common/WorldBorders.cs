using SpaceInvaders.Gameplay.Common;

namespace SpaceInvaders.Gameplay.Level {

    public class WorldBorders {

        public Axis Horizontal;
        public Axis Vertical;

        public WorldBorders(Axis horizontal, Axis vertical) {
            Horizontal = horizontal;
            Vertical = vertical;
        }
    }
}