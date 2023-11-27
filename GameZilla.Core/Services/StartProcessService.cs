using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GameZilla.Core.Contracts.Services;
using GameZilla.Core.Models.Emulateur;

namespace GameZilla.Core.Services;
public class StartProcessService : IStartProcessService
{
    private string programPath;
    private string PlateformeId;
    private static Process targetProcess;
    //private System.Timers.Timer processCheckTimer;
    static XInputWatcher watcher = new XInputWatcher();

    private readonly IParameterService _parameterService;
    public StartProcessService(IParameterService parameterService)
    {
        _parameterService = parameterService;
    }
    public void Init(string programPath, string plateformeId)
    {
        this.programPath = programPath;
        this.PlateformeId = plateformeId;
        targetProcess = null;
        //this.processCheckTimer = new System.Timers.Timer(1000); // Vérifiez toutes les secondes

        //// Configurez l'événement Elapsed du Timer
        //processCheckTimer.Elapsed += (sender, e) =>
        //{
        //    if (targetProcess != null && targetProcess.HasExited)
        //    {
        //        Console.WriteLine("Le programme cible s'est arrêté de manière inattendue.");
        //        targetProcess = null;

        //        // Vous pouvez prendre des mesures supplémentaires ici, par exemple redémarrer le programme cible.
        //    }
        //};

        // Démarrez le Timer
        //processCheckTimer.Start();
    }
    async Task IsEscapeCombinationSend(Process process)
    {
        await Task.Run(() =>
        {
            while (!process.HasExited)
            {
                watcher.Update();
                if (
                    (watcher.gamepad.Buttons == (SharpDX.XInput.GamepadButtonFlags.Start | SharpDX.XInput.GamepadButtonFlags.Back))
                  )
                {
                    process.Kill(true);
                    return;
                }
            }
        });

    }
    public async void StartExe()
    {
        if(this.PlateformeId == await _parameterService.GetParameterValue(Models.ParamEnum.SteamPlateformeId)
            || this.PlateformeId == await _parameterService.GetParameterValue(Models.ParamEnum.OriginPlateformeId)
            || this.PlateformeId == await _parameterService.GetParameterValue(Models.ParamEnum.EpicPlateformeId))
            {
                StartExeFromStore();
            }
        else { StartDirectExe(); }
    }

    private void StartExeFromStore()
    {
        bool isRunning = true;
        bool startPressed = false;
        bool selectPressed = false;

        // Exemple : Démarrer un programme cible (s'il n'est pas déjà en cours)
        if (targetProcess == null || targetProcess.HasExited)
        {
            try
            {
                //processCheckTimer.Start();
                var ps = new ProcessStartInfo(programPath)
                {
                    UseShellExecute = true,
                    Verb = "open"
                };
                targetProcess = Process.Start(ps);
                //await IsEscapeCombinationSend(targetProcess);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Erreur lors du démarrage du programme cible : " + ex.Message);
                return; // Sortez du thread si le démarrage a échoué
            }
        }
    }
    private async void StartDirectExe()
    {
        bool isRunning = true;
        bool startPressed = false;
        bool selectPressed = false;

        // Exemple : Démarrer un programme cible (s'il n'est pas déjà en cours)
        if (targetProcess == null || targetProcess.HasExited)
        {
            try
            {
                //processCheckTimer.Start();
                var ps = new ProcessStartInfo(programPath)
                {
                    UseShellExecute = true,
                    Verb = "open"
                };
                targetProcess = Process.Start(ps);
                await IsEscapeCombinationSend(targetProcess);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Erreur lors du démarrage du programme cible : " + ex.Message);
                return; // Sortez du thread si le démarrage a échoué
            }
        }
    }
}
