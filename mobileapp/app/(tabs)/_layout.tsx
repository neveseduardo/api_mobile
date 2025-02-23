import { Tabs } from 'expo-router';
import { Platform } from 'react-native';
import { HapticTab } from '@/components/HapticTab';
import TabBarBackground from '@/components/ui/TabBarBackground';
import { Colors } from '@/constants/Colors';
import { useColorScheme } from '@/hooks/useColorScheme';
import { AuthProvider } from '@/contexts/AuthContext';
import Ionicons from '@expo/vector-icons/Ionicons';
import '@/assets/styles/global.css';

export default function TabLayout() {
	const colorScheme = useColorScheme();

	return (
		<AuthProvider>
			<Tabs
				screenOptions={{
					tabBarActiveTintColor: Colors[colorScheme ?? 'light'].tint,
					headerShown: false,
					tabBarButton: HapticTab,
					tabBarBackground: TabBarBackground,
					tabBarStyle: Platform.select({
						ios: { position: 'absolute' },
						default: {},
					}),
				}}
			>
				<Tabs.Screen
					name="index"
					options={{
						title: 'Home',
						tabBarIcon: ({ color }) => <Ionicons name="home-outline" size={20} color={color} />,
					}}
				/>
				<Tabs.Screen
					name="search"
					options={{
						title: 'Pesquisar',
						tabBarIcon: ({ color }) => <Ionicons name="search" size={20} color={color} />,
					}}
				/>
				<Tabs.Screen
					name="schedules"
					options={{
						title: 'Agendamentos',
						tabBarIcon: ({ color }) => <Ionicons name="calendar-clear-outline" size={20} color={color} />,
					}}
				/>
				<Tabs.Screen
					name="units"
					options={{
						title: 'Unidades',
						tabBarIcon: ({ color }) => <Ionicons name="business-outline" size={20} color={color} />,
					}}
				/>
				<Tabs.Screen
					name="user"
					options={{
						title: 'Perfil',
						tabBarIcon: ({ color }) => <Ionicons name="person-outline" size={20} color={color} />,
					}}
				/>
			</Tabs>
		</AuthProvider>
	);
}
