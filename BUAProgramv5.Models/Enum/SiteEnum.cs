using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BUAProgramv5.Models.Enum
{
    public enum SiteEnum
    {
        [MultiDescriptionHelper("OU=Users,OU=131 Norwood,OU=Sites,DC=ad,DC=sumg,DC=int", "865-688-2522")]
        Norwood,
        [MultiDescriptionHelper("OU=Users,OU=115 Emory,OU=Sites,DC=ad,DC=sumg,DC=int", "865-938-3627")]
        Emory,
        [MultiDescriptionHelper("OU=Users,OU=118 Ftn City,OU=Sites,DC=ad,DC=sumg,DC=int", "865-687-1973")]
        FtnCity,
        [MultiDescriptionHelper("OU=Users,OU=124 IMA,OU=Sites,DC=ad,DC=sumg,DC=int", "865-546-9751")]
        IMA,
        [MultiDescriptionHelper("OU=Users,OU=139 Ft Sanders,OU=Sites,DC=ad,DC=sumg,DC=int", "865-524-1631")]
        FtSanders,
        [MultiDescriptionHelper("OU=Users,OU=130 Middle Creek,OU=Sites,DC=ad,DC=sumg,DC=int", "865-453-2039")]
        MiddleCreek,
        [MultiDescriptionHelper("OU=Users,OU=108 Bozeman,OU=Sites,DC=ad,DC=sumg,DC=int", "865-428-0583")]
        Bozeman,
        [MultiDescriptionHelper("OU=Users,OU=156 Summit Medical Group at Sevierville,OU=Sites,DC=ad,DC=sumg,DC=int", "865-428-0312")]
        SummitMedicalGroupatSevierville,
        [MultiDescriptionHelper("OU=Users,OU=150 TCIM,OU=Sites,DC=ad,DC=sumg,DC=int", "865-531-8848")]
        TCIM,
        [MultiDescriptionHelper("OU=Users,OU=110 Concord,OU=Sites,DC=ad,DC=sumg,DC=int", "865-691-0733")]
        Concord,
        [MultiDescriptionHelper("OU=Users,OU=117 Ft Loudon,OU=Sites,DC=ad,DC=sumg,DC=int", "865-986-4450")]
        FtLoudon,
        [MultiDescriptionHelper("OU=Users,OU=138 Farragut,OU=Sites,DC=ad,DC=sumg,DC=int", "865-966-3940")]
        Farragut,
        [MultiDescriptionHelper("OU=Users,OU=121 Halls,OU=Sites,DC=ad,DC=sumg,DC=int", "865-922-2121")]
        Halls,
        [MultiDescriptionHelper("OU=Users,OU=128 Gardner,OU=Sites,DC=ad,DC=sumg,DC=int", "865-983-0082")]
        Gardner,
        [MultiDescriptionHelper("OU=Users,OU=127 SMG at Maryville,OU=Sites,DC=ad,DC=sumg,DC=int", "865-982-7101")]
        SMGatMaryville,
        [MultiDescriptionHelper("OU=Users,OU=129 SMG at Parkwest,OU=Sites,DC=ad,DC=sumg,DC=int", "865-531-4600")]
        SMGatParkwest,
        [MultiDescriptionHelper("OU=Users,OU=126 Reese,OU=Sites,DC=ad,DC=sumg,DC=int", "865-577-9247")]
        Reese,
        [MultiDescriptionHelper("OU=Users,OU=152 SMG of Campbell County,OU=Sites,DC=ad,DC=sumg,DC=int", "423-566-8181")]
        SMGofCampbellCounty,
        [MultiDescriptionHelper("OU=Users,OU=137 Deane Hill,OU=Sites,DC=ad,DC=sumg,DC=int", "865-584-5762")]
        DeaneHill,
        [MultiDescriptionHelper("OU=Users,OU=148 Strawberry Plains,OU=Sites,DC=ad,DC=sumg,DC=int", "865-524-1661")]
        StrawberryPlains,
        [MultiDescriptionHelper("OU=Users,OU=120 Greeneville IMFM,OU=Sites,DC=ad,DC=sumg,DC=int", "423-638-4114")]
        GreenevilleIMFM,
        [MultiDescriptionHelper("OU=Users,OU=149 Summit Family Medicine,OU=Sites,DC=ad,DC=sumg,DC=int", "423-639-2161")]
        SummitFamilyMedicine,
        [MultiDescriptionHelper("OU=Users,OU=146 Oak Ridge,OU=Sites,DC=ad,DC=sumg,DC=int", "865-483-3172")]
        OakRidge,
        [MultiDescriptionHelper("OU=Users,OU=104 Razzak,OU=Sites,DC=ad,DC=sumg,DC=int", "865-453-5530")]
        Razzak,
        [MultiDescriptionHelper("OU=Users,OU=145 Clinton,OU=Sites,DC=ad,DC=sumg,DC=int", "865-457-8840")]
        Clinton,
        [MultiDescriptionHelper("OU=Users,OU=111 Cox,OU=Sites,DC=ad,DC=sumg,DC=int", "865-982-0170")]
        Cox,
        [MultiDescriptionHelper("OU=Users,OU=047 Isham,OU=Sites,DC=ad,DC=sumg,DC=int", "423-562-1181")]
        Isham,
        [MultiDescriptionHelper("OU=Users,OU=175 SEC Bearden,OU=Sites,DC=ad,DC=sumg,DC=int", "865-558-9822")]
        SECBearden,
        [MultiDescriptionHelper("OU=Users,OU=101 Central Lab,OU=Sites,DC=ad,DC=sumg,DC=int", "865-588-0236")]
        CentralLab,
        [MultiDescriptionHelper("OU=Users,OU=071 IMG Wellington,OU=Sites,DC=ad,DC=sumg,DC=int", "865-588-8005")]
        IMGWellington,
        [MultiDescriptionHelper("OU=Users,OU=067 IMG Cedar Bluff,OU=Sites,DC=ad,DC=sumg,DC=int", "865-588-8005")]
        IMGCedarBluff,
        [MultiDescriptionHelper("OU=Users,OU=9970 Accounting,OU=099 Central,OU=Sites,DC=ad,DC=sumg,DC=int", "865-584-4747")]
        CentralAccounting,
        [MultiDescriptionHelper("OU=Users,OU=9975 Accounts Receivable,OU=099 Central,OU=Sites,DC=ad,DC=sumg,DC=int", "865-584-4747")]
        CentralAccountsRecivable,
        [MultiDescriptionHelper("OU=Users,OU=9980 SSS Administration,OU=099 Central,OU=Sites,DC=ad,DC=sumg,DC=int", "865-584-4747")]
        SSSAdministration,
        [MultiDescriptionHelper("OU=Users,OU=9981 Provider Enrollment,OU=099 Central,OU=Sites,DC=ad,DC=sumg,DC=int", "865-584-4747")]
        ProviderEnrollment,
        [MultiDescriptionHelper("OU=Users,OU=9982 Operations,OU=099 Central,OU=Sites,DC=ad,DC=sumg,DC=int", "865-584-4747")]
        CentralOperations,
        [MultiDescriptionHelper("OU=Users,OU=9984 Recruiting,OU=099 Central,OU=Sites,DC=ad,DC=sumg,DC=int", "865-584-4747")]
        CentralRecruiting,
        [MultiDescriptionHelper("OU=Users,OU=9985 Board,OU=099 Central,OU=Sites,DC=ad,DC=sumg,DC=int", "865-584-4747")]
        Board,
        [MultiDescriptionHelper("OU=Users,OU=9986 Marketing & Communications,OU=099 Central,OU=Sites,DC=ad,DC=sumg,DC=int", "865-584-4747")]
        CentralMarketing,
        [MultiDescriptionHelper("OU=Users,OU=9990 Compliance,OU=099 Central,OU=Sites,DC=ad,DC=sumg,DC=int", "865-584-4747")]
        CentralCompliance,
        [MultiDescriptionHelper("OU=Users,OU=9987 SMG Administration,OU=099 Central,OU=Sites,DC=ad,DC=sumg,DC=int", "865-584-4747")]
        SMGAdministration,
        [MultiDescriptionHelper("OU=Users,OU=9995 Human Resources or TSOD,OU=099 Central,OU=Sites,DC=ad,DC=sumg,DC=int", "865-584-4747")]
        CentralHR,
        [MultiDescriptionHelper("OU=Users,OU=9999 Information Systems,OU=099 Central,OU=Sites,DC=ad,DC=sumg,DC=int", "865-584-4747")]
        CentralInformationSystems,
        [MultiDescriptionHelper("OU=Users,OU=050 Sleep Lab,OU=Sites,DC=ad,DC=sumg,DC=int", "865-909-0744")]
        SleepLab,
        [MultiDescriptionHelper("OU=Users,OU=177 SEC Ftn City,OU=Sites,DC=ad,DC=sumg,DC=int", "865-558-9822")]
        SECFtnCity,
        [MultiDescriptionHelper("OU=Users,OU=147 South Knox,OU=Sites,DC=ad,DC=sumg,DC=int", "865-579-0599")]
        SouthKnox,
        [MultiDescriptionHelper("OU=Users,OU=215 PC_Family Medicine Center,OU=Sites,DC=ad,DC=sumg,DC=int", "423-472-1511")]
        PCFamilyMedicineCenter,
        [MultiDescriptionHelper("OU=Users,OU=228 IMA Etowah,OU=Sites,DC=ad,DC=sumg,DC=int", "423-263-2444")]
        IMAEtowah,
        [MultiDescriptionHelper("OU=Users,OU=109 Concord Lenoir,OU=Sites,DC=ad,DC=sumg,DC=int", "865-986-3283")]
        ConcordLenoir,
        [MultiDescriptionHelper("OU=Users,OU=176 SEC Farragut,OU=Sites,DC=ad,DC=sumg,DC=int", "865-687-7704")]
        SECFarragut,
        [MultiDescriptionHelper("OU=Users,OU=142 Northshore,OU=Sites,DC=ad,DC=sumg,DC=int", "865-584-5762")]
        Northshore,
        [MultiDescriptionHelper("OU=Users,OU=119 Greeneville FM,OU=Sites,DC=ad,DC=sumg,DC=int", "423-638-1188")]
        GreenevilleFM,
        [MultiDescriptionHelper("OU=Users,OU=132 Thwing,OU=Sites,DC=ad,DC=sumg,DC=int", "423-639-0707")]
        Thwing,
        [MultiDescriptionHelper("OU=Users,OU=135 Seymour FP,OU=Sites,DC=ad,DC=sumg,DC=int", "865-577-4836")]
        SeymourFP,
        [MultiDescriptionHelper("OU=Users,OU=151 TN Valley,OU=Sites,DC=ad,DC=sumg,DC=int", "865-475-4742")]
        TNValley,
        [MultiDescriptionHelper("OU=Users,OU=116 Farragut Family,OU=Sites,DC=ad,DC=sumg,DC=int", "865-675-1953")]
        FarragutFamily,
        [MultiDescriptionHelper("OU=Users,OU=134 Rockwood,OU=Sites,DC=ad,DC=sumg,DC=int", "865-354-7799")]
        Rockwood,
        [MultiDescriptionHelper("OU=Users,OU=318 Bhandari,OU=Sites,DC=ad,DC=sumg,DC=int", "865-549-4900")]
        Bhandari,
        [MultiDescriptionHelper("OU=Users,OU=125 Proffitt,OU=Sites,DC=ad,DC=sumg,DC=int", "865-984-6823")]
        Proffitt,
        [MultiDescriptionHelper("OU=Users,OU=122 Hardin Valley,OU=Sites,DC=ad,DC=sumg,DC=int", "865-692-1220")]
        HardinValley,
        [MultiDescriptionHelper("OU=Users,OU=155 Tusculum,OU=Sites,DC=ad,DC=sumg,DC=int", "423-783-1965")]
        Tusculum,
        [MultiDescriptionHelper("OU=Users,OU=113 Passarello,OU=Sites,DC=ad,DC=sumg,DC=int", "865-522-6964")]
        Passarello,
        [MultiDescriptionHelper("OU=Users,OU=107 Caring Medical,OU=Sites,DC=ad,DC=sumg,DC=int", "865-992-2221")]
        CaringMedical,
        [MultiDescriptionHelper("OU=Users,OU=105 Vora,OU=Sites,DC=ad,DC=sumg,DC=int", "865-882-0105")]
        Vora,
        [MultiDescriptionHelper("OU=Users,OU=123 Hometown,OU=Sites,DC=ad,DC=sumg,DC=int", "423-442-5480")]
        Hometown,
        [MultiDescriptionHelper("OU=Users,OU=112 Sayani,OU=Sites,DC=ad,DC=sumg,DC=int", "865-982-4277")]
        Sayani,
        [MultiDescriptionHelper("OU=Users,OU=336 Family Medicine Greeneville,OU=Sites,DC=ad,DC=sumg,DC=int", "423-638-1188")]
        SiteThreeThreeSixFamilyMedicineGreeneville,
        [MultiDescriptionHelper("OU=Users,OU=141 Newport,OU=Sites,DC=ad,DC=sumg,DC=int", "865-475-4742")]
        Newport,
        [MultiDescriptionHelper("OU=Users,OU=114 West Knox,OU=Sites,DC=ad,DC=sumg,DC=int", "865-212-2282")]
        WestKnox,
        [MultiDescriptionHelper("OU=Users,OU=136 Seymour Med,OU=Sites,DC=ad,DC=sumg,DC=int", "865-577-5231")]
        SeymourMed,
        [MultiDescriptionHelper("OU=Users,OU=133 Powell Family,OU=Sites,DC=ad,DC=sumg,DC=int", "865-938-7517")]
        PowellFamily,
        [MultiDescriptionHelper("OU=Users,OU=144 Morristown,OU=Sites,DC=ad,DC=sumg,DC=int", "423-625-7777")]
        Morristown,
        [MultiDescriptionHelper("OU=Users,OU=143 Tellico,OU=Sites,DC=ad,DC=sumg,DC=int", "865-205-3029")]
        Tellico,
        [MultiDescriptionHelper("OU=Users,OU=345 Family Medicine Surgoinsville,OU=Sites,DC=ad,DC=sumg,DC=int", "423-345-2735")]
        Surgoinsville,
        [MultiDescriptionHelper("OU=Users,OU=140 Middlebrook,OU=Sites,DC=ad,DC=sumg,DC=int", "865-824-0033")]
        Middlebrook,
        [MultiDescriptionHelper("OU=Users,OU=165 Maryville Peds,OU=Sites,DC=ad,DC=sumg,DC=int", "865-581-0352")]
        MaryvillePeds,
        [MultiDescriptionHelper("OU=Users,OU=164 Shults,OU=Sites,DC=ad,DC=sumg,DC=int", "865-670-1560")]
        Shults,
        [MultiDescriptionHelper("OU=Users,OU=159 Hometown Pediatric Morristown,OU=Sites,DC=ad,DC=sumg,DC=int", "423-581-3904")]
        HometownPediatricMorristown,
        [MultiDescriptionHelper("OU=Users,OU=158 Hometown Pediatric Jefferson City,OU=Sites,DC=ad,DC=sumg,DC=int", "865-475-5377")]
        HometownPediatricJeffersonCity,
        [MultiDescriptionHelper("OU=Users,OU=160 Hometown Pediatric Sevierville,OU=Sites,DC=ad,DC=sumg,DC=int", "865-453-9980")]
        HometownPediatricSevierville,
        [MultiDescriptionHelper("OU=Users,OU=157 Children's Faith,OU=Sites,DC=ad,DC=sumg,DC=int", "865-690-8778")]
        ChildrensFaith,
        [MultiDescriptionHelper("OU=Users,OU=153 Trinity Knox,OU=Sites,DC=ad,DC=sumg,DC=int", "865-539-0270")]
        TrinityKnox,
        [MultiDescriptionHelper("OU=Users,OU=154 Trinity Maryville,OU=Sites,DC=ad,DC=sumg,DC=int", "865-982-0835")]
        TrinityMaryville,
        [MultiDescriptionHelper("OU=Users,OU=163 Pediatric Huxley,OU=Sites,DC=ad,DC=sumg,DC=int", "865-518-0084")]
        PediatricHuxley,
        [MultiDescriptionHelper("OU=Users,OU=189 Wartburg,OU=Sites,DC=ad,DC=sumg,DC=int", "423-346-3600")]
        Wartburg,
        [MultiDescriptionHelper("OU=Users,OU=161 Pediatric Hardin Valley,OU=Sites,DC=ad,DC=sumg,DC=int", "865-824-1522")]
        PediatricHardinValley,
        [MultiDescriptionHelper("OU=Users,OU=400 Summit Health Advantage,OU=Sites,DC=ad,DC=sumg,DC=int", "865-584-4747")]
        SummitHealthAdvantage,
        [MultiDescriptionHelper("OU=Users,OU=6010 SHS Admin,OU=400 Summit Health Advantage,OU=Sites,DC=ad,DC=sumg,DC=int", "865-584-4747")]
        SummitHealthAdvantageSHSAdmin,
        [MultiDescriptionHelper("OU=Users,OU=6015 Care Coordination,OU=400 Summit Health Advantage,OU=Sites,DC=ad,DC=sumg,DC=int", "865-584-4747")]
        SummitHealthAdvantageCareCoordination,
        [MultiDescriptionHelper("OU=Users,OU=6020 Chart Review,OU=400 Summit Health Advantage,OU=Sites,DC=ad,DC=sumg,DC=int", "865-584-4747")]
        SummitHealthAdvantageChartReview,
        [MultiDescriptionHelper("OU=Users,OU=9983 Quality,OU=400 Summit Health Advantage,OU=Sites,DC=ad,DC=sumg,DC=int", "865-584-4747")]
        SummitHealthAdvantageQuality,
        [MultiDescriptionHelper("OU=Users,OU=179 SEC Powell,OU=Sites,DC=ad,DC=sumg,DC=int", "865-824-1524")]
        SECPowell,
        [MultiDescriptionHelper("OU=Users,OU=070 Extensivist Clinic,OU=Sites,DC=ad,DC=sumg,DC=int", "865-824-1524")]
        ExtensivistClinic,
        [MultiDescriptionHelper("OU=Users,OU=162 Pediatric Seymour,OU=Sites,DC=ad,DC=sumg,DC=int", "865-205-3028")]
        PediatricSeymour,
        [MultiDescriptionHelper("OU=Users,OU=702 Summit Neurology,OU=Sites,DC=ad,DC=sumg,DC=int", "865-584-4747")]
        SummitNeurology,
        [MultiDescriptionHelper("OU=Users,OU=196 Athens,OU=Sites,DC=ad,DC=sumg,DC=int", "423-745-6575")]
        Athens,
        [MultiDescriptionHelper("OU=Users,OU=069 IMG Greeneville,OU=Sites,DC=ad,DC=sumg,DC=int", "423-636-2651")]
        IMGGreeneville,
        [MultiDescriptionHelper("OU=Users,OU=068 IMG Ftn City,OU=Sites,DC=ad,DC=sumg,DC=int", "865-588-8005")]
        IMGFtnCity,
        [MultiDescriptionHelper("OU=Users,OU=168 PT Ftn City,OU=Sites,DC=ad,DC=sumg,DC=int", "865-470-2696")]
        PTFtnCity,
        [MultiDescriptionHelper("OU=Users,OU=166 Cedar Bluff,OU=Sites,DC=ad,DC=sumg,DC=int", "865-470-2696")]
        CedarBluff,
        [MultiDescriptionHelper("OU=Users,OU=172 PT Powell,OU=Sites,DC=ad,DC=sumg,DC=int", "865-470-2696")]
        PTPowell,
        [MultiDescriptionHelper("OU=Users,OU=167 PT Farragut,OU=Sites,DC=ad,DC=sumg,DC=int", "865-470-2696")]
        PTFarragut,
        [MultiDescriptionHelper("OU=Users,OU=170 PT Jefferson City,OU=Sites,DC=ad,DC=sumg,DC=int", "865-470-2696")]
        PTJeffersonCity,
        [MultiDescriptionHelper("OU=999 Transcription,OU=Sites,DC=ad,DC=sumg,DC=int", "865-584-4747")]
        Transcription,
        [MultiDescriptionHelper("OU=Users,OU=CFP Float Pool,OU=Sites,DC=ad,DC=sumg,DC=int", "865-584-4747")]
        CFPFloatPool,
        [MultiDescriptionHelper("OU=NonSummit Accounts,OU=Sites,DC=ad,DC=sumg,DC=int", "865-584-4747")]
        NonSummitAccounts,
        [MultiDescriptionHelper("OU=Users,OU=219 Cool Springs,OU=Sites,DC=ad,DC=sumg,DC=int", "615-376-8195")]
        CoolSprings,
        [MultiDescriptionHelper("OU=Users,OU=220 North Franklin,OU=Sites,DC=ad,DC=sumg,DC=int", "615-435-3854")]
        NorthFranklin,
        [MultiDescriptionHelper("OU=Users,OU=241 PC_SMG Cookeville,OU=Sites,DC=ad,DC=sumg,DC=int", "931-783-4600")]
        PCSMGCookville,
        [MultiDescriptionHelper("OU=Users,OU=231 Goodlettsville Pediatrics,OU=Sites,DC=ad,DC=sumg,DC=int", "615-435-3854")]
        GoodlettsvillePediactrics,
        [MultiDescriptionHelper("OU=Users,OU=247 IMG Powell,OU=Sites,DC=ad,DC=sumg,DC=int", "615-435-3854")]
        IMGPowell,
        [MultiDescriptionHelper("OU=Users,OU=244 Shults Maryville,OU=Sites,DC=ad,DC=sumg,DC=int", "615-435-3854")]
        ShultsMaryville,
        [MultiDescriptionHelper("OU=Users,OU=266 Grace Primary Care,OU=Sites,DC=ad,DC=sumg,DC=int", "615-435-3854")]
        GracePrimaryCare,
        [MultiDescriptionHelper("OU=Users,OU=268 Upper Cumberland Family Physicians,OU=Sites,DC=ad,DC=sumg,DC=int", "615-435-3854")]
        UpperCumberlandFamilyPhysicians,
        [MultiDescriptionHelper("OU=Users,OU=250 EYE TEC Lenoir City,OU=Sites,DC=ad,DC=sumg,DC=int", "615-435-3854")]
        EYETECLeniorCity,
        [MultiDescriptionHelper("OU=Users,OU=251 EYE TEC Knoxville,OU=Sites,DC=ad,DC=sumg,DC=int", "615-435-3854")]
        EYETECKnoxville,
        [MultiDescriptionHelper("OU=Users,OU=252 EYE TEC Harriman,OU=Sites,DC=ad,DC=sumg,DC=int", "615-435-3854")]
        EYETECHarriman,
        [MultiDescriptionHelper("OU=Users,OU=253 EYE TEC Powell,OU=Sites,DC=ad,DC=sumg,DC=int", "615-435-3854")]
        EYETECPowell,
        [MultiDescriptionHelper("OU=Users,OU=254 EYE TEC Morristown,OU=Sites,DC=ad,DC=sumg,DC=int", "615-435-3854")]
        EYETECMorristown,
        [MultiDescriptionHelper("OU=Users,OU=270 Vora Satellite,OU=Sites,DC=ad,DC=sumg,DC=int", "931-456-0881")]
        VoraSatellite,
        [MultiDescriptionHelper("OU=Users,OU=269 Chattanooga Family Physicians,OU=Sites,DC=ad,DC=sumg,DC=int", "423-892-2221")]
        ChattanoogaFamilyPhysicians,
        [MultiDescriptionHelper("OU=Users,OU=258 TEC Hyde,OU=Sites,DC=ad,DC=sumg,DC=int", "800-500-4667")]
        TECHyde,
        [MultiDescriptionHelper("OU=Users,OU=278 Hermitage Primary Care,OU=Sites,DC=ad,DC=sumg,DC=int", "615-232-8812")]
        HermitagePrimaryCareSG,
        [MultiDescriptionHelper("OU=Users,OU=280 Brentwood Children's Clinic,OU=Sites,DC=ad,DC=sumg,DC=int", "615-261-1210")]
        BrentwoodChildrensClinic,
        [MultiDescriptionHelper("OU=Users,OU=285 KARM,OU=Sites,DC=ad,DC=sumg,DC=int", "865-329-9656")]
        KARM,
        [MultiDescriptionHelper("OU=Users,OU=286 Pistol Creek,OU=Sites,DC=ad,DC=sumg,DC=int", "865-984-6203")]
        PistolCreek
    }
}
