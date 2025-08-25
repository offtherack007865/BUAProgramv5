using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BUAProgramv5.Models.Enum
{
    public enum SecurityGroupEnum
    {
        [Description("Norwood SG")]
        Norwood,
        [Description("Emory Family Practice SG")]
        Emory,
        [Description("FCFP SG")]
        FountainCity,
        [Description("IMA SG")]
        IMA,
        [Description("Fort Sanders SG")]
        FortSanders,
        [Description("MCFP SG")]
        MiddleCreek,
        [Description("Bozeman SG")]
        Bozeman,
        [Description("Fry-Razzak SG")]
        Fry,
        [Description("TCIM SG")]
        TCIM,
        [Description("Concord SG")]
        Concord,
        [Description("Loudon SG")]
        Loudon,
        [Description("Farragut SG")]
        Farragut,
        [Description("Halls SG")]
        Halls,
        [Description("Chilhowee SG")]
        Chilhowee,
        [Description("Parkwest FP SG")]
        Parkwest,
        [Description("Cline SG")]
        Cline,
        [Description("MA SG")]
        MedicalAssociates,
        [Description("Reese SG")]
        Reese,
        [Description("LIMA SG")]
        TriCityFamilyPractice,
        [Description("Deane Hill SG")]
        DeanHill,
        [Description("Straw Plains SG")]
        StrawPlains,
        [Description("GIM SG")]
        Greenville,
        [Description("Family Medicine Greeneville SG")]
        FamilyMedicineGreeneville,
        [Description("Oak Ridge SG")]
        OakRidge,
        [Description("Fry-Razzak SG")]
        Razzak,
        [Description("Clinton SG")]
        Clinton,
        [Description("Cox SG")]
        Cox,
        [Description("Isham SG")]
        Isham,
        [Description("WUCC SG")]
        WestUrgentCareClinic,
        [Description("Lab SG")]
        CentralLab,
        [Description("Ancillary Wellington SG")]
        AncillaryWellington,
        [Description("Ancillary Cedar Bluff SG")]
        AncillaryCedarBluff,
        [Description("Accounting - Secure")]
        CentralAccounting,
        [Description("AR Employees")]
        CentralAccountsRecivable,
        [Description("SSS Administration")]
        SSSAdministration,
        [Description("Credentialing Department")]
        CentralCredentialing,
        [Description("Operations Support Team")]
        CentralOperations,
        [Description("Recruiting Department")]
        CentralRecruiting,
        [Description("Board")]
        Board,
        [Description("Marketing & Communications")]
        CentralMarketing,
        [Description("Compliance SG")]
        CentralCompliance,
        [Description("Human Resources SG")]
        CentralHR,
        [Description("Information Systems SG")]
        CentralInformationSystems,
        [Description("Sleep Center")]
        SleepCenter,
        [Description("UCC North")]
        UCCNorth,
        [Description("SMG of South Knoxville SG")]
        SMGSouthKnoxville,
        [Description("Concord SG")]
        ConcordatLenoirCity,
        [Description("Northshore SG")]
        Northshore,
        [Description("GFPA SG")]
        GFPA,
        [Description("Rheumatology SG")]
        Rheumatology,
        [Description("Thwing SG")]
        Thwing,
        [Description("Seymour SG")]
        Seymour,
        [Description("TN Valley MG SG")]
        TNValley,
        [Description("Farragut FP SG")]
        FarragutFP,
        [Description("Harriman SG")]
        RockwoodMedicalAssociates,
        [Description("Burkhart SG")]
        Burkhart,
        [Description("Bhandari SG")]
        Bhandari,
        [Description("Proffitt SG")]
        Proffitt,
        [Description("Harden Valley I.M. SG")]
        HardenValleyIM,
        [Description("Tusculum Family Physicians SG")]
        TusculumFamilyPhysicians,
        [Description("Passarello SG")]
        Passarello,
        [Description("Caring SG")]
        CaringMedicalCenter,
        [Description("Vora SG")]
        Vora,
        [Description("Kanabar SG")]
        Kanabar,
        [Description("Sayani SG")]
        Sayani,
        [Description("SMG Newport SG")]
        SMGNewport,
        [Description("West Knoxville Internal Medicine SG")]
        WestKnoxvilleInternalMedicine,
        [Description("Seymour Medical Center SG")]
        SeymourMedicalCenter,
        [Description("Powell Family Physicians SG")]
        PowellFamilyPhysicians,
        [Description("Morristown SG")]
        Morristown,
        [Description("SMG at Tellico SG")]
        SMGatTellico,
        [Description("Surgoinsville SG")]
        Surgoinsville,
        [Description("SMG Middlebrook  SG")]
        SMGMiddlebrook,
        [Description("SMG Maryville SG")]
        SMGMaryville,
        [Description("Shults Pediatrics SG")]
        ShultsPediatrics,
        [Description("HPA - Morristown SG")]
        HPAMorristown,
        [Description("HPA - Jefferson City SG")]
        HPAJeffersonCity,
        [Description("HPA - Sevierville SG")]
        HPASevierville,
        [Description("Childrens Faith Pediatrics SG")]
        ChildrensFaithPediatrics,
        [Description("GUCC SG")]
        UrgentCareGreeneville,
        [Description("Trinity Medical Knox SG")]
        TrinityMedicalKnox,
        [Description("Trinity Medical Maryville SG")]
        TrinityMEdicalMaryville,
        [Description("Pediatric Clinic - West Knoxville SG")]
        PediatricClinicWestKnox,
        [Description("Wartburg SG")]
        Wartburg,
        [Description("Pediatric Clinic - Karns SG")]
        PediatricClinicKarns,
        [Description("Summit Health Advantage SG")]
        SummitHealthAdvantage,
        [Description("SHS Admin SG")]
        SummitHealthAdvantageSHSAdmin,
        [Description("Care Coordination SG")]
        SummitHealthAdvantageCareCoordination,
        [Description("Chart Review SG")]
        SummitHealthAdvantageChartReview,
        [Description("VBC Quality SG")]
        SummitHealthAdvantageQuality,
        [Description("Express Powell SG")]
        ExpressPowell,
        [Description("Extensivist Clinic Powell SG")]
        ExtensivistClinicPowell,
        [Description("Pediatric Clinic - Seymour SG")]
        PeddiatricClinicSeymour,
        [Description("Summit Neurology SG")]
        SummitNeurology,
        [Description("Athens Medical Group SG")]
        AthensMedicalGroup,
        [Description("Greeneville Ancillary SG")]
        GreenevilleAncillary,
        [Description("Ancillary Ftn City SG")]
        FountainCityAncillary,
        [Description("Physical Therapy")]
        PTNorth,
        [Description("Physical Therapy")]
        PTWest,
        [Description("Punam Bhandari SG")]
        Transcription,
        [Description("Floater SG")]
        FloaterPool,
        [Description("NonSummitAccount SG")]
        NonSummitAccounts
    }
}
