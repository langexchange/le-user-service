using LE.UserService.Enums;
using System;

namespace LE.UserService.Helpers
{
    public class PostHelper
    {
        public static PostState GetState(int mode)
        {
            PostState state;
            switch (mode)
            {
                case 0:
                    state = PostState.Publish;
                    break;
                case 1:
                    state = PostState.Private;
                    break;
                case 2:
                    state = PostState.Delete;
                    break;
                case 3:
                    state = PostState.TurnOffComment;
                    break;
                case 4:
                    state = PostState.TurnOffShare;
                    break;
                case 5:
                    state = PostState.TurnOffCorrect;
                    break;
                case 6:
                    state = PostState.TurnOnShare;
                    break;
                case 7:
                    state = PostState.TurnOnCorrect;
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(mode));
            }
            return state;
        }

        public static string GetInteractMode(int mode)
        {
            string state;
            switch (mode)
            {
                case 0:
                    state = "Like";
                    break;
                case 1:
                    state = "UnLike";
                    break;
                case 2:
                    state = "Favorite";
                    break;
                default:
                    return "Like";
            }
            return state;
        }
    }
}
