namespace MyStudio.Services
{
    using Catel.MVVM;

    public interface ICommandsService
    {
        /// <summary>
        /// Gets command of canceling last user activity.
        /// </summary>
        Command UndoCommand { get; }

        /// <summary>
        /// Gets command of redo last canceled user activity.
        /// </summary>
        Command RedoCommand { get; }

        /// <summary>
        /// Gets command of opening new project.
        /// </summary>
        Command OpenProjectCommand { get; }

        /// <summary>
        /// Gets command of opening new project.
        /// </summary>
        Command SaveProjectCommand { get; } 
    }
}