#define AK_EMSCRIPTEN
namespace AK { class PluginRegistration; };
class AkUnityStaticPlugin;
AkUnityStaticPlugin * g_pAkUnityStaticPluginList = nullptr;
class AkUnityStaticPlugin {
	public:
	AkUnityStaticPlugin(AK::PluginRegistration* pReg) : m_pNext(g_pAkUnityStaticPluginList), m_pReg(pReg) { g_pAkUnityStaticPluginList = this; }
	AkUnityStaticPlugin *m_pNext;
	AK::PluginRegistration * m_pReg;
};
#define AK_STATIC_LINK_PLUGIN(_pluginName_) \
extern AK::PluginRegistration _pluginName_##Registration; \
AkUnityStaticPlugin _pluginName_##UnityStaticPlugin(&_pluginName_##Registration);
#include "AkPitchShifterFXFactory.h"
extern "C" {
	__attribute__ ((visibility("default"))) bool AkVerifyPluginRegistration() {
		bool bReg = true;
		AkUnityStaticPlugin * pNext = g_pAkUnityStaticPluginList;
		while (pNext != nullptr) { bReg = bReg && pNext->m_pReg != nullptr; pNext = pNext->m_pNext; }
		return bReg;
	}
}
