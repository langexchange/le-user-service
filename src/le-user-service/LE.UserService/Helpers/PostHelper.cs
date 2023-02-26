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
                default:
                    throw new ArgumentOutOfRangeException(nameof(mode));
            }
            return state;
        }
    }
}
