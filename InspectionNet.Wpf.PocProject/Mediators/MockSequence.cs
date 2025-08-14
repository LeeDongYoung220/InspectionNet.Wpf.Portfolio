using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using InspectionNet.Core.Enums;
using InspectionNet.Core.Managers;
using InspectionNet.Core.Mediators;
using InspectionNet.Core.Services;
using InspectionNet.Core.StaticClasses;
using InspectionNet.Core.VisionTools;
using InspectionNet.Wpf.PocProject.Configs;

namespace InspectionNet.Wpf.PocProject.Mediators
{
    public class MockSequence : ISequence
    {
        #region Variables

        private readonly IMotionService _motionService;
        private readonly IVisionMediator _visionMediator;
        private readonly IRuleToolService _toolRuleService;
        private readonly ILogManager _logManager;
        private int _alignStep;
        private OperationStatus _operationStatus;
        private readonly int _initPos = 0;

        #endregion

        #region Properties
        public bool IsToolRun { get; set; }

        #endregion

        #region Events
        public event EventHandler<ILpToolResult> VisionToolGroupCompleted;
        #endregion

        #region Constructor

        public MockSequence(IMotionService motionService,
                            IVisionMediator visionMediator,
                            IRuleToolService toolService,
                            ILogManager logManager)
        {
            _motionService = motionService;
            _motionService.MoveCompleted += MotionService_MoveCompleted;
            _visionMediator = visionMediator;
            _toolRuleService = toolService;
            _logManager = logManager;
            _visionMediator.SelectedVisionRuleToolGroupCompleted += VisionMediator_SelectedVisionRuleToolGroupCompleted;
        }


        #endregion

        #region Finalizer

        #endregion

        #region Methods

        private void VisionMediator_SelectedVisionRuleToolGroupCompleted(object sender, ILpToolResult e)
        {
            if (_operationStatus == OperationStatus.Align)
            {
                // IsAlignOK false인 경우 재이동
                if (!IsAlignOK(e, out double resultX, out double resultY))
                {
                    _motionService.MoveTarget(resultX, resultY, true);
                }
                // IsAlignOK true인 경우 현재 좌표 저장 스텝 증가, 시퀀스 재실행
                else
                {
                    _motionService.SetAlignFinalPosition(_alignStep);
                    _alignStep++;
                    RunAlignSequence();
                }
            }

            VisionToolGroupCompleted?.Invoke(this, e);
        }

        private void RunAlignSequence()
        {
            switch (_alignStep)
            {
                // Align Start
                case 0:
                    _motionService.MoveAlignPosition(_initPos);
                    break;

                // 첫번 째 얼라인 완료 후 2번째 위치로 이동
                case 1:
                    _motionService.MoveAlignPosition(1);
                    break;
                // 두번째 얼라인 완료 후 1번,2번 위치 중간값으로 이동
                case 2:
                    MoveToAlignedMidPoint();
                    break;
                default:
                    break;
            }
        }

        private void MoveToAlignedMidPoint()
        {
            var posL = _motionService.AlignPositions.ElementAtOrDefault(0);
            var posR = _motionService.AlignPositions.ElementAtOrDefault(1);
            _toolRuleService.CreateFixture(posL.FinalX, posL.FinalY, posR.FinalX, posR.FinalY);
            _toolRuleService.ConvertPositionInFixture(0, 0, out double outputX, out double outputY);
            _motionService.MoveTarget(outputX, outputY);
            _logManager.LogInfo(this, $"Align positions: L({posL.FinalX}, {posL.FinalY}), R({posR.FinalX}, {posR.FinalY})");
        }

        private static bool IsAlignOK(ILpToolResult e, out double resultX, out double resultY)
        {
            resultX = ToolHelper.GetTerminalValueDouble(e, TerminalNameConstants.OUTPUTX);
            resultY = ToolHelper.GetTerminalValueDouble(e, TerminalNameConstants.OUTPUTY);
            return IsSpecOK(resultX, resultY);
        }

        private static bool IsSpecOK(double x, double y)
        {
            double specX = 20;
            double specY = 20;
            if (Math.Abs(x) < specX && Math.Abs(y) < specY) return true;
            return false;
        }

        private void MotionService_MoveCompleted(object sender, EventArgs e)
        {
            switch (_operationStatus)
            {
                case OperationStatus.Align:
                    switch (_alignStep)
                    {
                        // 첫번째 그랩 위치 도착
                        case 0:
                            _visionMediator.Run();
                            break;

                        // 두번째 그랩 위치 도착
                        case 1:
                            _visionMediator.Run();
                            break;
                        case 2:
                            _operationStatus = OperationStatus.AlignCompleted;
                            break;
                        default:
                            break;
                    }
                    break;

                case OperationStatus.Run:
                    _visionMediator.Run();
                    break;
            }
        }

        // 얼라인 시작 위치로 이동
        public void StartAlign()
        {
            _alignStep = 0;
            _operationStatus = OperationStatus.Align;
            RunAlignSequence();
        }

        public void Run()
        {
            
        }

        public void Stop()
        {
            
        }
        #endregion
    }
}
