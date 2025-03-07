import { Tabs } from 'expo-router';
import { Platform } from 'react-native';
import TabBarBackground from '@/components/ui/TabBarBackground';
import { AuthProvider } from '@/contexts/UserAuthenticationContext';
import Ionicons from '@expo/vector-icons/Ionicons';
import { HapticTab } from '@/components/ui/HapticTab';
import { SafeAreaView } from 'react-native-safe-area-context';

export default function TabLayout() {

	return (
		<SafeAreaView className="flex-1">
			<AuthProvider>
				<Tabs
					screenOptions={{
						headerShown: false,
						tabBarButton: HapticTab,
						tabBarBackground: TabBarBackground,
						tabBarStyle: Platform.select({
							ios: { position: 'absolute' },
							default: {},
						}),
					}}
				>
					<Tabs.Screen name="index" options={{
						title: 'Home',
						tabBarIcon: ({ color }) => <Ionicons name="home-outline" size={20} color={color} />,
					}}
					/>
					<Tabs.Screen name="exames" options={{
						title: 'Exames',
						tabBarIcon: ({ color }) => <Ionicons name="flask-outline" size={20} color={color} />,
					}}
					/>
					<Tabs.Screen name="agendamentos" options={{
						title: 'Agendamentos',
						tabBarIcon: ({ color }) => <Ionicons name="calendar-clear-outline" size={20} color={color} />,
					}}
					/>
					<Tabs.Screen name="unidades" options={{
						title: 'Unidades',
						tabBarIcon: ({ color }) => <Ionicons name="business-outline" size={20} color={color} />,
					}}
					/>
					<Tabs.Screen name="perfil" options={{
						title: 'Perfil',
						tabBarIcon: ({ color }) => <Ionicons name="person-outline" size={20} color={color} />,
					}}
					/>
				</Tabs>
			</AuthProvider>
		</SafeAreaView>

	);
}
