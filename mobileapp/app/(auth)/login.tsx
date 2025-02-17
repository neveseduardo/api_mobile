import { useState } from 'react';
import { View, Text } from 'react-native';
import { Href, router } from 'expo-router';
import { ThemedView } from '@/components/ThemedView';
import Button from '@/components/ui/Button';
import TextInput from '@/components/ui/TextInput';
import PasswordInput from '@/components/ui/PasswordInput';
import { ThemedText } from '@/components/ThemedText';
import Ionicons from '@expo/vector-icons/Ionicons';
import { useAuth } from '@/contexts/AuthContext';

export default function LoginScreen() {
	const [email, setEmail] = useState('email@email.com');
	const [password, setPassword] = useState('Senh@123');
	const [loading, setLoading] = useState(false);
	const { login } = useAuth();

	const handleLogin = async () => {
		try {
			setLoading(true);
			const { access_token, userData } = await login(email, password);

			if (access_token && userData) {
				router.replace('/(tabs)' as Href);
			}
		} catch (error) {
			console.error(error, email, password);
		} finally {
			setLoading(false);
		}
	};

	return (
		<ThemedView className="items-center flex-1 p-8">
			<View className="flex flex-col w-full gap-5">
				<View className="w-full flex justify-center items-center h-[200px]">
					<View className="w-[70px] h-[70px] bg-primary-500 rounded-full flex justify-center items-center">
						<Ionicons name="lock-closed-outline" size={32} className="text-white" />
					</View>
				</View>

				<View className="flex flex-row items-center w-full gap-2">
					<ThemedText className="text-2xl text-white">Autenticação</ThemedText>
				</View>

				<View className="w-full">
					<ThemedText className="text-white">Insira os dados solicitados abaixo para autenticar no aplicativo.</ThemedText>
				</View>

				<View className="flex flex-col w-full gap-4">
					<TextInput
						keyboardType="email-address"
						returnKeyType="next"
						placeholder="Email"
						value={email}
						onChangeText={setEmail}
						disabled={loading}
					/>

					<PasswordInput
						placeholder="Senha"
						returnKeyType="done"
						value={password}
						onChangeText={setPassword}
						disabled={loading}
					/>

					<Button
						onPress={handleLogin}
						color="primary"
						className="w-full"
						disabled={loading}
					>
						<Text className="text-white">ENTRAR</Text>
					</Button>

					<Button
						onPress={() => router.push('/(auth)/register')}
						className="w-full"
						disabled={loading}
					>
						<ThemedText>REGISTRAR</ThemedText>
					</Button>
				</View>
			</View>
		</ThemedView>
	);
}