using EWTSS_DESKTOP.Core.Models;
using EWTSS_DESKTOP.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using ScenarioModel = EWTSS_DESKTOP.Core.Models.Scenario;
using MessageBox = System.Windows.MessageBox;

namespace EWTSS_DESKTOP.Presentation.Views.Scenario
{
    public partial class ScenarioEditorPage
    {
        private void LoadScenario()
        {
            try
            {
                using var db = new AppDbContext();

                var scenario = db.Scenarios
                    .Where(x => x.Id == _scenarioId)
                    .Select(x => new
                    {
                        Scenario = x,
                        UserFirstName = x.User != null ? x.User.FirstName : null,
                        UserLastName = x.User != null ? x.User.LastName : null
                    })
                    .FirstOrDefault();

                if (scenario == null)
                {
                    MessageBox.Show("Scenario not found.");
                    return;
                }

                var scenarioData = scenario.Scenario;

                ScenarioTitleTextBlock.Text = $"{scenarioData.Name} | - TREE STRUCTURE";
                UserNameTextBlock.Text = !string.IsNullOrWhiteSpace(scenario.UserFirstName)
                    ? $"{scenario.UserFirstName} {scenario.UserLastName}"
                    : "Admin Admin";

                ScenarioNameTextBox.Text = scenarioData.Name ?? string.Empty;
                ScenarioDescriptionTextBox.Text = scenarioData.Description ?? string.Empty;
                ScenarioCreatedDateTextBlock.Text = $"SCENARIO CREATION DATE (DD-MM-YYYY) : {scenarioData.StartDate:dd-MM-yyyy}";
                ScenarioCreatedTimeTextBlock.Text = $"SCENARIO CREATION TIME (HH:MM:SS) : {scenarioData.StartTime}";
                ScenarioDurationTextBox.Text = scenarioData.Duration.ToString();
                ScenarioExecutionTimeTextBox.Text = scenarioData.ExecuteTime?.ToString() ?? "--:--:--";

                UpdateDescriptionCount();
                LoadScenarioTree(scenarioData);
                InitializeStkViews(scenarioData.Name ?? "Scenario");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Failed to load scenario.\n\n{ex.Message}");
            }
        }

        private void LoadTablePreview()
        {
            var rows = new List<DrsPreviewRow>
            {
                new() { Sno = 1, Side = "RED",  EmitterName = "COMEMITTER1",        EmitterType = "CUSTOMIZED", Mode = "FF",    Modulation = "AM" },
                new() { Sno = 2, Side = "BLUE", EmitterName = "COMMEMITTERALLIED1", EmitterType = "CUSTOMIZED", Mode = "FH",    Modulation = ""   },
                new() { Sno = 3, Side = "BLUE", EmitterName = "RDFS1",              EmitterType = "CUSTOMIZED", Mode = "SCAN",  Modulation = ""   },
                new() { Sno = 4, Side = "BLUE", EmitterName = "JSVUSHF1",           EmitterType = "CUSTOMIZED", Mode = "FF",    Modulation = "AM" },
                new() { Sno = 5, Side = "RED",  EmitterName = "RADAR1",             EmitterType = "CUSTOMIZED", Mode = "TRACK", Modulation = ""   }
            };

            DrsDataGrid.ItemsSource = rows;
        }

        // keep your existing methods here
        private void LoadAreaOperationFromDb()
        {
            using var db = new AppDbContext();

            var area = db.AreaOperations
                .Where(x => x.ScenarioId == _scenarioId)
                .OrderByDescending(x => x.Id)
                .FirstOrDefault();

            ShowAreaOfOperationForm();

            if (area == null)
            {
                _selectedAreaOperationId = null;

                SetAreaOperationButtonState(false); // CREATE

                return;
            }

            _selectedAreaOperationId = area.Id;

            // load fields if needed
            // AreaOperationNameTextBox.Text = area.Name;

            SetAreaOperationButtonState(true); // UPDATE
        }
        private void LoadCcFromDbByName(string ccName)
        {
            using var db = new AppDbContext();

            var cc = db.Ccs
                .Where(x => x.Line.AreaOperation.ScenarioId == _scenarioId)
                .AsEnumerable()
                .FirstOrDefault(x =>
                    string.Equals(
                        x.CcName?.Trim(),
                        ccName.Trim(),
                        StringComparison.OrdinalIgnoreCase));

            if (cc == null)
            {
                _selectedCcId = null;
                CcNameTextBox.Text = ccName;
                CcLatitudeTextBox.Text = "";
                CcLongitudeTextBox.Text = "";
                ShowCcPropertiesForm();
                return;
            }

            _selectedCcId = cc.Id;
            CcNameTextBox.Text = cc.CcName ?? "";
            CcLatitudeTextBox.Text = cc.Latitude ?? "";
            CcLongitudeTextBox.Text = cc.Longitude ?? "";

            ShowCcPropertiesForm();
        }
        private void LoadRdfsFromDbByName(string entityName)
        {
            using var db = new AppDbContext();

            var entity = db.Entities
                .Where(x => x.Cc.Line.AreaOperation.ScenarioId == _scenarioId
                         && x.EntityType == EntityType.RDFS)
                .AsEnumerable()
                .FirstOrDefault(x =>
                    string.Equals(
                        x.Name?.Trim(),
                        entityName.Trim(),
                        StringComparison.OrdinalIgnoreCase));

            if (entity == null)
            {
                _selectedEntityId = null;

                // keep your old default form structure and default values
                ShowRdfsPropertiesForm(entityName);
                RdfsNameTextBox.Text = entityName;

                SetRdfsButtonState(false);   // CREATE
                return;
            }

            _selectedEntityId = entity.Id;
            _selectedCcId = entity.CcId;

            // first load normal form structure
            ShowRdfsPropertiesForm(entity.Name ?? "RDFS");

            // then replace defaults with DB values
            RdfsNameTextBox.Text = entity.Name ?? "RDFS1";
            RdfsMinFreqTextBox.Text = entity.StartFrequencyValue?.ToString() ?? "435";
            RdfsMaxFreqTextBox.Text = entity.StopFrequencyValue?.ToString() ?? "1000";
            RdfsAntennaTypeTextBox.Text = entity.AntennaType ?? "Default";
            RdfsPolarizationTextBox.Text = entity.Polarization ?? "Vertical";
            RdfsAntennaHeightTextBox.Text = entity.AntennaHeight?.ToString() ?? "10";
            RdfsScanTypeTextBox.Text = entity.ScanType ?? "SCAN";

            SetRdfsButtonState(true);   // UPDATE
        }

        private void LoadJsvushfFromDbByName(string entityName)
        {
            using var db = new AppDbContext();

            var entity = db.Entities
                .Where(x => x.Cc.Line.AreaOperation.ScenarioId == _scenarioId
                         && x.EntityType == EntityType.JSVUSHF)
                .AsEnumerable()
                .FirstOrDefault(x =>
                    string.Equals(
                        x.Name?.Trim(),
                        entityName.Trim(),
                        StringComparison.OrdinalIgnoreCase));

            if (entity == null)
            {
                _selectedEntityId = null;

                ShowJsvushfPropertiesForm(entityName);

                JsvushfNameTextBox.Text = entityName;
                JsvushfMinFreqTextBox.Text = "";
                JsvushfMaxFreqTextBox.Text = "";
                JsvushfAntennaTypeTextBox.Text = "";
                JsvushfPolarizationTextBox.Text = "";
                JsvushfAntennaHeightTextBox.Text = "";
                JsvushfScanTypeTextBox.Text = "";

                return;
            }

            _selectedEntityId = entity.Id;
            _selectedCcId = entity.CcId;

            ShowJsvushfPropertiesForm(entity.Name ?? "JSVUSHF");

            JsvushfNameTextBox.Text = entity.Name ?? string.Empty;
            JsvushfMinFreqTextBox.Text = entity.StartFrequencyValue?.ToString() ?? string.Empty;
            JsvushfMaxFreqTextBox.Text = entity.StopFrequencyValue?.ToString() ?? string.Empty;
            JsvushfAntennaTypeTextBox.Text = entity.AntennaType ?? string.Empty;
            JsvushfPolarizationTextBox.Text = entity.Polarization ?? string.Empty;
            JsvushfAntennaHeightTextBox.Text = entity.AntennaHeight?.ToString() ?? string.Empty;
            JsvushfScanTypeTextBox.Text = entity.ScanType ?? string.Empty;
        }
        private void LoadCommEmitterAlliedFromDbByName(string emitterName)
        {
            using var db = new AppDbContext();

            var emitter = db.Emitters
                .Where(x => x.Line.AreaOperation.ScenarioId == _scenarioId
                         && x.Type == EmitterKeyName.COMMEMITTERALLIED)
                .AsEnumerable()
                .FirstOrDefault(x =>
                    string.Equals(
                        x.Name?.Trim(),
                        emitterName.Trim(),
                        StringComparison.OrdinalIgnoreCase));

            if (emitter == null)
            {
                _selectedEmitterId = null;

                ShowCommEmitterAlliedPropertiesForm(emitterName);

                CommEmitterNameTextBox.Text = emitterName;
                CommEmitterPowerTextBox.Text = "";
                CommEmitterBandwidthTextBox.Text = "";
                CommEmitterFrequencyTextBox.Text = "";
                CommEmitterAntennaTypeTextBox.Text = "";
                CommEmitterPolarizationTextBox.Text = "";
                CommEmitterScanRateTextBox.Text = "";
                CommEmitterGainTextBox.Text = "";

                return;
            }

            _selectedEmitterId = emitter.Id;

            ShowCommEmitterAlliedPropertiesForm(emitter.Name ?? "COMMEMITTERALLIED");

            CommEmitterNameTextBox.Text = emitter.Name ?? string.Empty;
            CommEmitterPowerTextBox.Text = emitter.PowerTransmitted?.ToString() ?? string.Empty;
            CommEmitterBandwidthTextBox.Text = emitter.Bandwidth?.ToString() ?? string.Empty;
            CommEmitterFrequencyTextBox.Text = emitter.StartFrequencyValue?.ToString() ?? string.Empty;
            CommEmitterAntennaTypeTextBox.Text = emitter.AntennaType ?? string.Empty;
            CommEmitterPolarizationTextBox.Text = emitter.Polarization ?? string.Empty;
            CommEmitterScanRateTextBox.Text = emitter.ScanRate?.ToString() ?? string.Empty;
            CommEmitterGainTextBox.Text = emitter.Gain?.ToString() ?? string.Empty;
        }
        private void LoadRadarEmitterAlliedFromDbByName(string emitterName)
        {
            using var db = new AppDbContext();

            var emitter = db.Emitters
                .Where(x => x.Line.AreaOperation.ScenarioId == _scenarioId
                         && x.Type == EmitterKeyName.RADAREMITTERALLIED)
                .AsEnumerable()
                .FirstOrDefault(x =>
                    string.Equals(
                        x.Name?.Trim(),
                        emitterName.Trim(),
                        StringComparison.OrdinalIgnoreCase));

            if (emitter == null)
            {
                _selectedEmitterId = null;

                ShowRadarEmitterAlliedPropertiesForm(emitterName);

                RadarEmitterNameTextBox.Text = emitterName;
                RadarEmitterPowerTextBox.Text = "";
                RadarEmitterBandwidthTextBox.Text = "";
                RadarEmitterFrequencyTextBox.Text = "";
                RadarEmitterAntennaTypeTextBox.Text = "";
                RadarEmitterPolarizationTextBox.Text = "";
                RadarEmitterScanRateTextBox.Text = "";
                RadarEmitterGainTextBox.Text = "";

                return;
            }

            _selectedEmitterId = emitter.Id;

            ShowRadarEmitterAlliedPropertiesForm(emitter.Name ?? "RADAREMITTERALLIED");

            RadarEmitterNameTextBox.Text = emitter.Name ?? string.Empty;
            RadarEmitterPowerTextBox.Text = emitter.PowerTransmitted?.ToString() ?? string.Empty;
            RadarEmitterBandwidthTextBox.Text = emitter.Bandwidth?.ToString() ?? string.Empty;
            RadarEmitterFrequencyTextBox.Text = emitter.StartFrequencyValue?.ToString() ?? string.Empty;
            RadarEmitterAntennaTypeTextBox.Text = emitter.AntennaType ?? string.Empty;
            RadarEmitterPolarizationTextBox.Text = emitter.Polarization ?? string.Empty;
            RadarEmitterScanRateTextBox.Text = emitter.ScanRate?.ToString() ?? string.Empty;
            RadarEmitterGainTextBox.Text = emitter.Gain?.ToString() ?? string.Empty;
        }
    }
}